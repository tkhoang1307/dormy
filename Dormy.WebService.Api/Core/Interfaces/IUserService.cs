using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse> SignUp(UserRequestModel model);
        Task<ApiResponse> Login(LoginRequestModel model);
        Task<ApiResponse> GetUserBatch(GetBatchRequestModel request);
        Task<ApiResponse> GetUserInformation(Guid id);
        Task<ApiResponse> ChangePassword(ChangePasswordRequestModel model);
        Task<ApiResponse> UpdateProfile(Guid id, UserUpdateRequestModel model);
        Task<ApiResponse> ResetPassword(Guid id);
        Task<ApiResponse> UpdateUserWorkplace(Guid workplaceId, Guid userId);
        Task<ApiResponse> UpdateUserHealthInsurance(HealthInsuranceRequestModel? model, Guid userId);
        Task<ApiResponse> GetUserProfileByUseridForAdmin(Guid id);
    }
}
