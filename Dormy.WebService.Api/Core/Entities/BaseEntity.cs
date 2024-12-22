namespace Dormy.WebService.Api.Core.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }
        public Guid LastUpdatedBy { get; set; }
        public uint RecordVersion { get; set; }

    }
}
