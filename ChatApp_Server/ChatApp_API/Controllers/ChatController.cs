using ChatApp_API.Services.IServices;
using ChatApp_API.Services.WebSocketServices;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChatController : ControllerBase
	{
		private readonly ChatWebSocketHandler _chatWebSocketHandler;
		private readonly IApplicationUserService _applicationUserService;

		public ChatController(ChatWebSocketHandler chatWebSocketHandler, IApplicationUserService applicationUserService)
		{
			_chatWebSocketHandler = chatWebSocketHandler;
			_applicationUserService = applicationUserService;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] Guid userId)
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				var user = await _applicationUserService.GetUserAsync(userId);
				if (user is null)
				{
					return BadRequest("User not found!");
				}

				await _chatWebSocketHandler.HandleWebSocketAsync(HttpContext, user);
				return new EmptyResult();
			}
			else
			{
				return BadRequest("WebSocket is not supported.");
			}
		}
	}
}
