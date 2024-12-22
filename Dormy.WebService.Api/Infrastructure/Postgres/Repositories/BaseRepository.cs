using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            T? entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter)
        {
            if (filter != null)
            {
                return await _dbSet.Where(filter).ToListAsync();
            }
            return await _dbSet.ToListAsync();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            else
            {

                return _dbSet.FirstOrDefaultAsync(filter);
            }
        }

        public async Task<bool> IsExisted(Guid id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
