using ChatApp_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_API.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<ApplicationUser> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>().HasKey(user => user.Id);
		}
	}
}
