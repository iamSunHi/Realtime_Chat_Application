using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Services.WebSocketServices
{
	public class ChatWebSocketHandler
	{
		private Guid USER_ID = Guid.Empty;
		private readonly string SOCKET_SERVER_URL = "wss://localhost:9999/api/chat";
		public readonly ClientWebSocket ClientSocket;

		public ChatWebSocketHandler(Guid userId)
		{
			ClientSocket = new ClientWebSocket();
			USER_ID = userId;
		}

		public async Task<(bool, string)> ConnectSocketServerAsync()
		{
			try
			{
				await ClientSocket.ConnectAsync(new Uri($"{SOCKET_SERVER_URL}?userId={USER_ID}"), CancellationToken.None);
				return (true, "Connected to server.");
			}
			catch (Exception ex)
			{
				return (false, $"Connection failed: {ex.Message}");
			}
		}

		public async Task<string> SendMessageAsync(string message)
		{
			try
			{
				var buffer = Encoding.UTF8.GetBytes(message);
				await ClientSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
				return "Message sent.";
			}
			catch (Exception ex)
			{
				return $"Failed to send message: {ex.Message}";
			}
		}

		public async Task DisconnectSocketServerAsync()
		{
			await ClientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnected.", CancellationToken.None);
		}
	}
}