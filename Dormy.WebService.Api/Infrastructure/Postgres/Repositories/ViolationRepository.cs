using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class ViolationRepository : BaseRepository<ViolationEntity>, IViolationRepository
    {
        public ViolationRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
