using ChatApp_API.DTOs;
using ChatApp_API.Models;
using ChatApp_API.Repositories.IRepositories;
using System.Net.WebSockets;
using System.Text;

namespace ChatApp_API.Services.WebSocketServices
{
	public class ChatWebSocketHandler
	{
		private readonly ILogger<ChatWebSocketHandler> _logger;
		private readonly WebSocketConnectionManager _connectionManager;

		public ChatWebSocketHandler(ILogger<ChatWebSocketHandler> logger, WebSocketConnectionManager connectionManager)
		{
			_logger = logger;
			_connectionManager = connectionManager;
		}

		public async Task HandleWebSocketAsync(HttpContext httpContext, UserInfoDTO user)
		{

			var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
			_connectionManager.AddSocket(webSocket, user.Id);

			_logger.LogInformation($"Client with ID {user.Id} connected!");
			await BroadcastMessageAsync($"admin: {user.Name} has joined the chat session.", senderId: Guid.Empty);

			try
			{
				while (webSocket.State == WebSocketState.Open)
				{
					var message = await ReceiveMessageAsync(webSocket);

					if (!string.IsNullOrEmpty(message))
					{
						_logger.LogInformation($"Received message from client with ID {user.Id}: {message}");

						message = $"{user.Name}: {message}";
						await BroadcastMessageAsync(message, senderId: user.Id);
					}
				}
			}
			catch { }
			finally
			{
				_connectionManager.RemoveSocket(user.Id);
				_logger.LogError($"Client with ID {user.Id} disconnected!");
				await BroadcastMessageAsync($"admin: {user.Name} has left the chat session.", senderId: Guid.Empty);
			}
		}

		private async Task<string> ReceiveMessageAsync(WebSocket webSocket)
		{
			var buffer = new byte[1024];
			var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

			if (result.CloseStatus.HasValue)
			{
				return string.Empty;
			}

			return Encoding.UTF8.GetString(buffer, 0, result.Count);
		}

		private async Task BroadcastMessageAsync(string message, Guid senderId)
		{
			var (userIdList, socketList) = _connectionManager.GetAllSockets();

			for (int i = 0; i < socketList.Length; i++)
			{
				if (socketList[i].State == WebSocketState.Open && userIdList[i] != senderId)
				{
					await socketList[i].SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
				}
			}
		}
	}
}
