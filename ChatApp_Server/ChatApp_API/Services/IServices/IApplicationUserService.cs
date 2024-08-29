using ChatApp_API.Models;

namespace ChatApp_API.Services.IServices
{
	public interface IApplicationUserService
	{
		public Task<ApplicationUser?> GetUserAsync(Guid userId);
	}
}
