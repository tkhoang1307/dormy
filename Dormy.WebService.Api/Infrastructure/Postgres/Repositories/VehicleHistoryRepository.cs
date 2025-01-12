using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class VehicleHistoryRepository : BaseRepository<VehicleHistoryEntity>, IVehicleHistoryRepository
    {
        public VehicleHistoryRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
