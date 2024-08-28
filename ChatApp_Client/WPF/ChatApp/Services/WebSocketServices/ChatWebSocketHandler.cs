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
		private readonly string SOCKET_SERVER_URL = "wss://localhost:9999/api/chat";
		public readonly ClientWebSocket ClientSocket;

		public ChatWebSocketHandler()
		{
			ClientSocket = new ClientWebSocket();
		}

		public async Task<string> ConnectSocketServer()
		{
			try
			{
				await ClientSocket.ConnectAsync(new Uri(SOCKET_SERVER_URL), CancellationToken.None);
				return "Connected to server.";
			}
			catch (Exception ex)
			{
				return $"Connection failed: {ex.Message}";
			}
		}

		public async Task<string> SendMessage(string message)
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
	}
}