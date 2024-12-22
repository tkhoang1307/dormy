using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class AdminRepository : BaseRepository<AdminEntity>, IAdminRepository
    {
        public AdminRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
