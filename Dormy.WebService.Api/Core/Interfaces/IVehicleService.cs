using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IVehicleService
    {
        Task<ApiResponse> AddNewVehicle(VehicleRequestModel model);
        Task<ApiResponse> UpdateVehicle(VehicleUpdationRequestModel model);
        Task<ApiResponse> GetDetailVehicleById(Guid id);
        Task<ApiResponse> GetVehicleBatch(GetBatchVehicleRequestModel model);
        Task<ApiResponse> GetAllVehiclesOfUser();
        Task<ApiResponse> SoftDeleteVehicle(Guid id);
    }
}
