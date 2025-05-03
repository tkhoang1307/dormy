namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ChangePasswordRequestModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequestModel
    {
        public string Email { get; set; } = string.Empty;
    }
}
