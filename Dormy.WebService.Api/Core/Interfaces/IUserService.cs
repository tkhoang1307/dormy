﻿using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse> SignUp(UserRequestModel model);
        Task<ApiResponse> Login(LoginRequestModel model);
        Task<ApiResponse> GetUserBatch(bool isGetAll, List<Guid> ids);
        Task<ApiResponse> GetUserInformation(Guid id);
        Task<ApiResponse> ChangePassword(Guid id, string newPassword);
        Task<ApiResponse> UpdateProfile(Guid id, UserUpdateRequestModel model);
    }
}
