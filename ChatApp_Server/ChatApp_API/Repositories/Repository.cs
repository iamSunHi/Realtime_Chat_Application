using ChatApp_API.Data;
using ChatApp_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatApp_API.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly ApplicationDbContext _context;
		internal DbSet<T> _dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			this._dbSet = _context.Set<T>();
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp.Trim());
				}
			}
			return await query.ToListAsync();
		}

		public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;
			query = query.Where(filter);

			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp.Trim());
				}
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task CreateAsync(T entity)
		{
			using var transaction = _context.Database.BeginTransaction();

			try
			{
				await _dbSet.AddAsync(entity);
				await SaveAsync();

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task RemoveAsync(T entity)
		{
			using var transaction = _context.Database.BeginTransaction();

			try
			{
				_dbSet.Remove(entity);
				await SaveAsync();

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
