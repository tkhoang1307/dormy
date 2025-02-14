namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class GuardianResponseModel : BaseUserResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string RelationshipToUser { get; set; } = string.Empty;

        public UserResponseModel User { get; set; } = null!;
    }
}
