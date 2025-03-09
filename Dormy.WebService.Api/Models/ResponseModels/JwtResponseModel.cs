namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class JwtResponseModel
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
