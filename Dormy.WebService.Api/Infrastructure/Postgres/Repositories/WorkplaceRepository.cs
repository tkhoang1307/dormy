using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class WorkplaceRepository : BaseRepository<WorkplaceEntity>, IWorkplaceRepository
    {
        public WorkplaceRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
