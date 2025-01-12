using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class RoomRepository : BaseRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
