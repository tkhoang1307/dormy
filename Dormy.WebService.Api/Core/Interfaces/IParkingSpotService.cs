using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IParkingSpotService
    {
        Task<ApiResponse> AddNewParkingSpot(ParkingSpotRequestModel model);
        Task<ApiResponse> UpdateParkingSpot(ParkingSpotUpdationRequestModel model);
        Task<ApiResponse> UpdateStatusParkingSpot(ParkingSpotUpdateStatusRequestModel model);
        Task<ApiResponse> GetParkingSpotBatch(GetBatchRequestModel model);
        Task<ApiResponse> GetDetailParkingSpot(Guid id);
        Task<ApiResponse> SoftDeleteParkingSpot(Guid id);
    }
}
