using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class AdminRepository : BaseRepository<AdminEntity>, IAdminRepository
    {
        public AdminRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<(AdminEntity?, AdminEntity?)> GetAuthors(Guid? createdUserId, Guid? lastUpdatedUserId)
        {
            AdminEntity? createdUser = null;
            AdminEntity? lastUpdatedUser = null;

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
