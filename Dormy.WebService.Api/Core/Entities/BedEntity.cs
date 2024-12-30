namespace Dormy.WebService.Api.Core.Entities
{
    public class BedEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Need to check again data type
        public string BedNumber { get; set; } = string.Empty;

        public Guid RoomId { get; set; }

        public RoomEntity Room { get; set; } = null!;
    }
}
