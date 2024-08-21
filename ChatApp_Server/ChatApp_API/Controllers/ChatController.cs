using ChatApp_API.Services.WebSocketServices;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChatController : ControllerBase
	{
		private readonly ChatWebSocketHandler _chatWebSocketHandler;

		public ChatController(ChatWebSocketHandler chatWebSocketHandler)
		{
			_chatWebSocketHandler = chatWebSocketHandler;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
				await _chatWebSocketHandler.HandleWebSocketAsync(webSocket);
				return new EmptyResult();
			}
			else
			{
				return BadRequest("WebSocket is not supported.");
			}
		}
	}
}
