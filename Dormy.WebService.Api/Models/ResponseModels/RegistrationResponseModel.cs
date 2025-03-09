namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RegisterModel
    {
        public Guid CcontractId { get; set; }
        public UserLoginResponseModel User { get; set; } = null!;
    }
}
