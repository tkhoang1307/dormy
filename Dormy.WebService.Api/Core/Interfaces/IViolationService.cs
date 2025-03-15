using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IViolationService
    {
        Task<ApiResponse> CreateViolation(ViolationRequestModel model);
        Task<ApiResponse> GetSingleViolation(Guid id);
        Task<ApiResponse> GetViolationBatch(GetBatchRequestModel model);
        Task<ApiResponse> UpdateViolation(ViolationUpdationRequestModel model);
        Task<ApiResponse> SoftDeleteViolation(Guid id);
    }
}
