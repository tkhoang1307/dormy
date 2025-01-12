using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class BedRepository : BaseRepository<BedEntity>, IBedRepository
    {
        public BedRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
