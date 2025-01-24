using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface ISettingService
    {
        Task<ApiResponse> CreateSetting(SettingRequestModel model);
        Task<ApiResponse> UpdateSetting(SettingUpdateRequestModel model);
        Task<ApiResponse> GetSettings();
        Task<ApiResponse> GetSettingById(Guid id);
        Task<ApiResponse> SoftDeleteSetting(Guid id);
    }
}
