using ChatApp_API.DTOs;
using ChatApp_API.Models;

namespace ChatApp_API.Services.IServices
{
	public interface IApplicationUserService
	{
		public Task<UserInfoDTO?> GetUserAsync(Guid userId);
	}
}
