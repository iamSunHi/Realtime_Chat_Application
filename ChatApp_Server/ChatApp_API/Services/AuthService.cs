using ChatApp_API.DTOs;
using ChatApp_API.Models;
using ChatApp_API.Repositories.IRepositories;
using ChatApp_API.Services.IServices;
using System.Text.RegularExpressions;

namespace ChatApp_API.Services
{
	public class AuthService : IAuthService
	{
		private readonly IApplicationUserRepository _userRepository;

		public AuthService(IApplicationUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Guid> Login(string username, string password)
		{
			var user = await _userRepository.GetAsync(user => user.Username.ToLower() == username.ToLower() && user.Password == password);

			if (user == null)
			{
				return Guid.Empty;
			}
			else
			{
				return user.Id;
			}
		}

		public async Task<bool> Register(RegisterDTO registerDTO)
		{
			if (registerDTO.Email != null && !Regex.IsMatch(registerDTO.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
			{
				return false;
			}

			if (!_userRepository.IsUniqueUser(registerDTO.Username))
			{
				return false;
			}

			var newUser = new ApplicationUser
			{
				Id = Guid.NewGuid(),
				Name = registerDTO.Name,
				Email = registerDTO.Email,
				Username = registerDTO.Username,
				Password = registerDTO.Password,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};

			try
			{
				await _userRepository.CreateAsync(newUser);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
