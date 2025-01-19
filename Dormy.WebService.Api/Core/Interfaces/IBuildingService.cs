using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IBuildingService
    {
        Task<ApiResponse> CreateBuilding(BuildingRequestModel model);
        Task<ApiResponse> GetBuildingById(Guid id);
        Task<ApiResponse> GetBuildingBatch(List<Guid> ids, bool isGetAll);
        Task<ApiResponse> SoftDeleteBuildingById(Guid id);
    }
}
