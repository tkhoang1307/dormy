namespace Dormy.WebService.Api.Core.Entities
{
    public class WorkplaceEntity: BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;

        public List<UserEntity> Users { get; set; } = [];
    }
}
