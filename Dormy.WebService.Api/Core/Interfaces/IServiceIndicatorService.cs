using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IServiceIndicatorService
    {
        Task<ApiResponse> AddServiceIndicator(ServiceIndicatorRequestModel model);
        Task<ApiResponse> AddBatchServiceIndicators(ServiceIndicatorCreationBatchRequestModel model);
        Task<ApiResponse> UpdateServiceIndicator(ServiceIndicatorUpdationRequestModel model);
        Task<ApiResponse> GetDetailServiceIndicatorById(Guid id);
        Task<ApiResponse> GetServiceIndicatorBatch(GetBatchServiceIndicatorRequestModel model);
        Task<ApiResponse> HardDeleteBatchServiceIndicators(List<Guid> ids);
    }
}
