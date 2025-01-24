namespace Dormy.WebService.Api.Core.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDateUtc { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedDateUtc { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? LastUpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
