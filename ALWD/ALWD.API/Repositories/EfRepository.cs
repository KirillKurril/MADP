using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;

namespace ALWD.API.Data.Repository
{
    public class EfRepository<T> : IRepository<T> where T : DbEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _entities;
        public EfRepository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

		public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default,
	        params Expression<Func<T, object>>[]? includesProperties)
		{
			IQueryable<T> query = _entities.AsQueryable();

			if (includesProperties?.Any() == true)
			{
				foreach (Expression<Func<T, object>> included in includesProperties)
				{
					query = query.Include(included);
				}
			}

			return await query.SingleAsync(i => i.Id == id, cancellationToken);
		}
		public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<T>? query = _entities.AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includesProperties)
        {
            IQueryable<T>? query = _entities.AsQueryable();
            if (includesProperties.Any())
            {
                foreach (Expression<Func<T, object>>? included in
               includesProperties)
                {
                    query = query.Include(included);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync(cancellationToken);
		}

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Remove(entity);
			await _context.SaveChangesAsync(cancellationToken);
		}

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            IQueryable<T>? query = _entities.AsQueryable();
            T first = await query.FirstOrDefaultAsync(filter);
            return first;
        }

    }
}
