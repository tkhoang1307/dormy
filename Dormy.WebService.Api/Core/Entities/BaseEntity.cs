namespace Dormy.WebService.Api.Core.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDateUtc { get; set; } = DateTime.Now;

        public DateTime LastUpdatedDateUtc { get; set; } = DateTime.Now;

        public Guid CreatedBy { get; set; } = Guid.NewGuid();

        public Guid LastUpdatedBy { get; set; } = Guid.NewGuid();

        public bool isDeleted { get; set; } = false;

    }
}
