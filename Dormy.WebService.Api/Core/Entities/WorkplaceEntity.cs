namespace Dormy.WebService.Api.Core.Entities
{
    public class WorkplaceEntity: BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public UserEntity User { get; set; } = null!;
    }
}
