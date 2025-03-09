using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IRequestService
    {
        Task<ApiResponse> CreateRequest(RequestRequestModel model);
        Task<ApiResponse> GetRequests(GetBatchRequestModel model);
        Task<ApiResponse> GetSingleRequest(Guid id);
        Task<ApiResponse> UpdateRequest(Guid id, RequestRequestModel model);
        Task<ApiResponse> UpdateRequestStatus(Guid id, RequestStatusEnum status);
    }
}
