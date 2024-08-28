using ChatApp_API.Models;

namespace ChatApp_API.Repositories.IRepositories
{
	public interface IApplicationUserRepository : IRepository<ApplicationUser>
	{
		bool IsUniqueUser(string username);
		Task Update(ApplicationUser user);
	}
}
