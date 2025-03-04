using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class RoomRepository : BaseRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<string> GetRoomName(Guid? roomId)
        {
            RoomEntity? room = null;

            if (roomId != null)
            {
                room = await _dbSet.FirstOrDefaultAsync(x => x.Id == roomId);
            }

            return room?.RoomNumber.ToString();
        }
    }
}
