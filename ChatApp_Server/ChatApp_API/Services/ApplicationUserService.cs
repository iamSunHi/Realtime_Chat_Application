using ChatApp_API.Models;
using ChatApp_API.Repositories.IRepositories;
using ChatApp_API.Services.IServices;

namespace ChatApp_API.Services
{
	public class ApplicationUserService : IApplicationUserService
	{
		private readonly IApplicationUserRepository _applicationUserRepository;

		public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
		{
			_applicationUserRepository = applicationUserRepository;
		}

		public async Task<ApplicationUser?> GetUserAsync(Guid userId)
		{
			return await _applicationUserRepository.GetAsync(u => u.Id == userId);
		}
	}
}
