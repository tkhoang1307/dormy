using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IVehicleHistoryService
    {
        Task<ApiResponse> CreateVehicleHistory(VehicleHistoryRequestModel model);
        Task<ApiResponse> GetVehicleHistories(GetBatchRequestModel model);
        Task<ApiResponse> GetVehicleHistoriesByVehicleId(Guid vehicleId);
        Task<ApiResponse> SoftDeleteVehicleHistory(Guid id);
    }
}
