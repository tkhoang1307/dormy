using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class RoomTypeRepository : BaseRepository<RoomTypeEntity>, IRoomTypeRepository
    {
        public RoomTypeRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
