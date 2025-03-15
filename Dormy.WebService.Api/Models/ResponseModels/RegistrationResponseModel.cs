namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RegisterModel
    {
        public Guid ContractId { get; set; }
        public UserLoginResponseModel User { get; set; } = null!;
    }
}
