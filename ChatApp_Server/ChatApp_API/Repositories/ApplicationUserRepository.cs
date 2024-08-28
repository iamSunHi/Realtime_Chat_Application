using ChatApp_API.Data;
using ChatApp_API.Models;
using ChatApp_API.Repositories.IRepositories;

namespace ChatApp_API.Repositories
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
	{
		public ApplicationUserRepository(ApplicationDbContext context) : base(context)
		{
		}

		public bool IsUniqueUser(string username)
		{
			return !_context.Users.Any(u => u.Username.ToLower().Trim() == username.ToLower().Trim());
		}

		public async Task Update(ApplicationUser user)
		{
			using var transaction = _context.Database.BeginTransaction();

			try
			{
				_context.Users.Update(user);
				await SaveAsync();

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}
	}
}
