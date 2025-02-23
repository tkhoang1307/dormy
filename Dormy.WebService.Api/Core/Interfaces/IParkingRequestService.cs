using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IParkingRequestService
    {
        Task<ApiResponse> CreateParkingRequest(ParkingRequestModel model);
        Task<ApiResponse> UpdateParkingRequest(UpdateParkingRequestModel model);
        Task<ApiResponse> UpdateParkingRequestStatus(ParkingRequestStatusModel model);
        Task<ApiResponse> GetSingleParkingRequest(Guid id);
        Task<ApiResponse> GetParkingRequestBatch(List<Guid> ids, bool isGetAll = false);
        Task<ApiResponse> SoftDeleteParkingRequest(Guid id);
    }
}
