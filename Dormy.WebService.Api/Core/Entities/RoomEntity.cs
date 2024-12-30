namespace Dormy.WebService.Api.Core.Entities
{
    public class RoomEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string RoomNumer { get; set; } = string.Empty;

        public int FloorNumber { get; set; } 

        public int Capacity { get; set; }

        // Shuold define enum for this status
        public string Status { get; set; } = string.Empty;

        public Guid BuildingId { get; set; }

        public BuildingEntity Building { get; set; } = null!;

        public Guid RoomTypeId { get; set; }

        public RoomTypeEntity RoomType { get; set; } = null!;

        public List<BedEntity>? Beds { get; set; }
    }
}
