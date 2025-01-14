namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class AdminLoginResponseModel
    {
        public AdminResponseModel AdminInformation { get; set; } = null!;

        public string AccessToken { get; set; } = string.Empty;
    }
}
