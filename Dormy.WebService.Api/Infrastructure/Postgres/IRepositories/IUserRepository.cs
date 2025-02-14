using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Infrastructure.Postgres.IRepositories
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<(UserEntity?, UserEntity?)> GetAuthors(Guid? createdUserId, Guid? lastUpdatedUserId);
    }
}
