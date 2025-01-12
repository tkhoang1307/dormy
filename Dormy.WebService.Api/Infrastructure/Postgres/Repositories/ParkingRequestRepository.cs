using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class ParkingRequestRepository : BaseRepository<ParkingRequestEntity>, IParkingRequestRepository
    {
        public ParkingRequestRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
