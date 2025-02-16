using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IRoomTypeServiceService
    {
        Task<ApiResponse> AddRoomTypeService(RoomTypeServiceCreationRequestModel model);
        Task<ApiResponse> RemoveRoomTypeService(RoomTypeServiceDeletionRequestModel model);
    }
}
