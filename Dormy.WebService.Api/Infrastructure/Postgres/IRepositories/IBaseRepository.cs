using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Dormy.WebService.Api.Infrastructure.Postgres.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        Task DeleteByIdAsync(Guid id);
        Task<bool> IsExisted(Guid id);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool isNoTracking = false);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter,
                                               Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, int pageIndex = 1, int pageSize = 25, bool isPaging = false, bool isNoTracking = true);
    }
}
