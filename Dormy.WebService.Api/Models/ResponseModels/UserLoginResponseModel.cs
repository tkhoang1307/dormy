namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class UserLoginResponseModel
    {
        public UserResponseModel UserInformation { get; set; } = null!;

        public string AccessToken { get; set; } = string.Empty;
    }
}
