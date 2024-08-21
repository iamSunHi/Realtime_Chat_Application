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

		public async Task HandleWebSocketAsync(WebSocket webSocket)
		{
			var socketId = _connectionManager.AddSocket(webSocket);

			_logger.LogInformation($"Client with ID {socketId} connected!");

			try
			{
				while (webSocket.State == WebSocketState.Open)
				{
					var message = await ReceiveMessageAsync(webSocket);

					if (!string.IsNullOrEmpty(message))
					{
						_logger.LogInformation($"Received message from client with ID {socketId}: {message}");

						await BroadcastMessageAsync(message);
					}
				}
			}
			catch { }
			finally
			{
				_connectionManager.RemoveSocket(socketId);
				_logger.LogError($"Client with ID {socketId} disconnected!");
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

		private async Task BroadcastMessageAsync(string message)
		{
			foreach (var socket in _connectionManager.GetAllSockets())
			{
				if (socket.State == WebSocketState.Open)
				{
					await socket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
				}
			}
		}
	}
}
