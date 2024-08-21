using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ChatApp_API.Services.WebSocketServices
{
	public class WebSocketConnectionManager
	{
		private readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new();

		public WebSocket GetSocketById(Guid id)
		{
			return _sockets.FirstOrDefault(p => p.Key == id).Value;
		}

		public Guid GetSocketId(WebSocket socket)
		{
			return _sockets.FirstOrDefault(p => p.Value == socket).Key;
		}

		public Guid AddSocket(WebSocket socket)
		{
			var id = Guid.NewGuid();
			_sockets.TryAdd(id, socket);
			return id;
		}

		public IEnumerable<WebSocket> GetAllSockets()
		{
			return _sockets.Values;
		}

		public void RemoveSocket(Guid socketId)
		{
			_sockets.TryRemove(socketId, out _);
		}
	}
}
