namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ChangePasswordRequestModel
    {
        public Guid? Id { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
