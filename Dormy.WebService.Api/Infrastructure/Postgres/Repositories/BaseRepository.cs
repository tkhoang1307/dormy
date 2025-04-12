using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            T? entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter,
                                               Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                               int pageIndex = 1,
                                               int pageSize = 25,
                                               bool isPaging = false,
                                               bool isNoTracking = true)
        {
            IQueryable<T> query = _dbSet;


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (isPaging)
            {
                query = query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize); ;
            }

            if (isNoTracking)
            {
                return await query.AsNoTracking().ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool isNoTracking = false)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                query = include(query);
            }

            if (isNoTracking)
            {
                return await query.AsNoTracking().FirstOrDefaultAsync(filter);
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<bool> IsExisted(Guid id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
