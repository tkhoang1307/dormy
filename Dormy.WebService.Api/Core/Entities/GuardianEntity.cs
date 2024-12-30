namespace Dormy.WebService.Api.Core.Entities
{
    public class GuardianEntity: BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string RelationshipToUser { get; set; } = string.Empty;

        public UserEntity User { get; set; } = null!; 
    }
}
