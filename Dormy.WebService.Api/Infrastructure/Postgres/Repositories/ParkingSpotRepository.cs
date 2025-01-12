using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class ParkingSpotRepository : BaseRepository<ParkingSpotEntity>, IParkingSpotRepository
    {
        public ParkingSpotRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
