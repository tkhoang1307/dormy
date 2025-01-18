using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IWorkplaceService
    {
        Task<ApiResponse> CreateWorkplace(WorkplaceRequestModel model);
        Task<ApiResponse> UpdateWorkplace(WorkplaceUpdateRequestModel model);
        Task<ApiResponse> GetSingleWorkplaceById(Guid id);
        Task<ApiResponse> GetWorkplaceBatch(List<Guid> ids);
        Task<ApiResponse> GetAllWorkplace(int pageIndex, int pageSize);
        Task<ApiResponse> SoftDeleteWorkplace(Guid id);
        Task<ApiResponse> HardDeleteWorkplace(Guid id);
    }
}
