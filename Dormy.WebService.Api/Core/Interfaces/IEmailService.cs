using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendVerifyEmailAsync(string email);
        Task<ApiResponse> SendForgotPasswordEmailAsync(string email);
    }
}
