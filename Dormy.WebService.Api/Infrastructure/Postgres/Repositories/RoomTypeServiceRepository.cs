using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class RoomTypeServiceRepository : BaseRepository<RoomTypeServiceEntity>, IRoomTypeServiceRepository
    {
        public RoomTypeServiceRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
