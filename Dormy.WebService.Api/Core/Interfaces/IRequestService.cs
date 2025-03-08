using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IRequestService
    {
        Task<ApiResponse> GetRequests(GetBatchRequestModel model);
        Task<ApiResponse> GetSingleRequest(Guid id);
        Task<ApiResponse> UpdateRequestStatus(Guid id, RequestStatusEnum status);
    }
}
