using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class VehicleRepository : BaseRepository<VehicleEntity>, IVehicleRepository
    {
        public VehicleRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
