using ChatApp_API.DTOs;
using ChatApp_API.Models;
using ChatApp_API.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
		{
			var userId = await _authService.Login(loginDTO.Username, loginDTO.Password);

			if (userId == Guid.Empty)
			{
				return Unauthorized();
			}
			else
			{
				return Ok(userId);
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
		{
			var isSuccess = await _authService.Register(registerDTO);

			if (isSuccess)
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
