using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface ISettingService
    {
        Task<ApiResponse> CreateSetting(SettingRequestModel model);
        Task<ApiResponse> UpdateSetting(SettingUpdateValueRequestModel model);
        Task<ApiResponse> TurnOnOrTurnOffSetting(SettingTurnOnOffRequestModel model);
        Task<ApiResponse> GetAllSettings();
        Task<ApiResponse> GetSettingByKeyName(string keyname);
        Task<ApiResponse> HardDeleteSettingByKeyName(string keyname);
        Task<ApiResponse> GetAllDataTypeEnums();
    }
}
