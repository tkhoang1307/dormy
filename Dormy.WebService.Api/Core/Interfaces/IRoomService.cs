using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IRoomService
    {
        Task<ApiResponse> CreateRoomBatch(List<RoomRequestModel> rooms, Guid buildingId);
        Task<ApiResponse> GetRoomsByBuildingId(Guid buildingId);
        Task<ApiResponse> GetRoomById(Guid id);
        Task<ApiResponse> UpdateRoom(RoomUpdateRequestModel rooms);
        Task<ApiResponse> UpdateRoomStatus(RoomUpdateStatusRequestModel rooms);
        Task<ApiResponse> SoftDeleteRoom(Guid id);
    }
}
