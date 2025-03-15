using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IRoomServiceService
    {
        Task<ApiResponse> AddRoomServiceBatch(List<RoomServiceRequestModel> models);
        Task<ApiResponse> AddRoomService(RoomServiceRequestModel model);
        Task<ApiResponse> UpdateRoomService(RoomServiceUpdateRequestModel model);
        Task<ApiResponse> GetRoomServiceBatch(GetBatchRequestModel model);
        Task<ApiResponse> GetRoomSeviceSingle(Guid id);
        Task<ApiResponse> SoftDeleteRoomServiceBatch(List<Guid> ids);
    }
}
