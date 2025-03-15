namespace Dormy.WebService.Api.Models.RequestModels
{
    public class GuardianRequestModel
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string RelationshipToUser { get; set; } = string.Empty;

        public Guid? UserId { get; set; }
    }

    public class GuardianUpdationRequestModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string RelationshipToUser { get; set; } = string.Empty;
    }
}
