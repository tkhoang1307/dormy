using System.Linq.Expressions;

namespace Dormy.WebService.Api.Infrastructure.Postgres.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);
        Task DeleteByIdAsync(Guid id);
        Task<bool> IsExisted(Guid id);
    }
}
