namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendVerifyEmailAsync(string email);
    }
}
