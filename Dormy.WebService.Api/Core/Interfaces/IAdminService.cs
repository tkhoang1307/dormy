using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IAdminService
    {
        Task<ApiResponse> CreateAdminAccount(AdminRequestModel model);
        Task<AdminEntity?> Login(LoginRequestModel model);
        Task<List<AdminResponseModel>> GetAllUser();
        Task<ApiResponse> GetAdminAccount(Guid id);
        Task<ApiResponse> ChangeAdminPassword(Guid id, string newPassword);
    }
}
