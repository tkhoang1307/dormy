using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class RoomServiceRepository : BaseRepository<RoomServiceEntity>, IRoomServiceRepository
    {
        public RoomServiceRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
