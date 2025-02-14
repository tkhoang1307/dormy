using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<(UserEntity?, UserEntity?)> GetAuthors(Guid? createdUserId, Guid? lastUpdatedUserId)
        {
            UserEntity? createdUser = null;
            UserEntity? lastUpdatedUser = null;

            if (createdUserId != null)
            {
                createdUser = await _dbSet.FirstOrDefaultAsync(x => x.Id == createdUserId);
            }

            if (lastUpdatedUserId != null)
            {
                lastUpdatedUser = await _dbSet.FirstOrDefaultAsync(x => x.Id == lastUpdatedUserId);
            }

            return (createdUser, lastUpdatedUser);
        }
    }
}
