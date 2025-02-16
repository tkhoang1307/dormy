using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IGuardianService
    {
        Task<ApiResponse> AddNewGuardian(GuardianRequestModel model);
        Task<ApiResponse> UpdateGuardian(GuardianUpdationRequestModel model);
        Task<ApiResponse> GetAllGuardiansOfUser();
        Task<ApiResponse> GetDetailGuardianById(Guid id);
        Task<ApiResponse> GetGuardianBatch(GetBatchGuardianRequestModel model);
        Task<ApiResponse> HardDeleteParkingSpot(Guid id);
    }
}
