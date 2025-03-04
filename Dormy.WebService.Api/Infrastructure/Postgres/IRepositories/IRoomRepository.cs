using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Infrastructure.Postgres.IRepositories
{
    public interface IRoomRepository : IBaseRepository<RoomEntity>
    {
        Task<string> GetRoomName(Guid? roomId);
    }
}
