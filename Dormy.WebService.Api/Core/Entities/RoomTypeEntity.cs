namespace Dormy.WebService.Api.Core.Entities
{
    public class RoomTypeEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string RoomTypeName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal Price { get; set; }

        public List<RoomEntity>? Rooms { get; set; }

        public List<RoomTypeServiceEntity>? RoomTypeServices { get; set; }
    }
}
