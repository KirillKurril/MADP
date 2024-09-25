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

        public async Task<bool> Exists(Expression<Func<T, bool>> filter)
        {
            IQueryable<T>? query = _entities.AsQueryable();
            bool exists = await query.AnyAsync();
            return exists;
        }
        public async Task<bool> Exists(int id)
        {
            try
            {
                await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
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

            T entity;
            try
            {
                entity = await query.SingleAsync(i => i.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T)} with id {id} doesn't exist, method: GetByTdAsync, class: EfRepository");
            }
            return entity;

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
            var trackedEntity = await _context.Set<T>().FindAsync(new object[] { entity.Id }, cancellationToken);

            if (trackedEntity == null)
            {
                throw new InvalidOperationException("Entity not found in the database.");
            }

            _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
            _context.Entry(trackedEntity).State = EntityState.Modified;

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
