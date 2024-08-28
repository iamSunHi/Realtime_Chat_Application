using ChatApp_API.DTOs;

namespace ChatApp_API.Services.IServices
{
	public interface IAuthService
	{
		Task<Guid> Login(string username, string password);
		Task<bool> Register(RegisterDTO registerDTO);
	}
}
