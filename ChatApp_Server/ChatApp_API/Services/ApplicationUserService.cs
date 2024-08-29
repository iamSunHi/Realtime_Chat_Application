using ChatApp_API.DTOs;
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

		public async Task<UserInfoDTO?> GetUserAsync(Guid userId)
		{
			var userFromDb = await _applicationUserRepository.GetAsync(u => u.Id == userId);

			if (userFromDb is null)
			{
				return null;
			}

			return new UserInfoDTO
			{
				Id = userFromDb.Id,
				Username = userFromDb.Username,
				Email = userFromDb.Email,
				Name = userFromDb.Name,
				CreatedAt = userFromDb.CreatedAt,
				UpdatedAt = userFromDb.UpdatedAt
			};
		}
	}
}
