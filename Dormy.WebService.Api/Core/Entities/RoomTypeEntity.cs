namespace Dormy.WebService.Api.Core.Entities
{
    public class RoomTypeEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Type { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal Price { get; set; }

        public List<RoomEntity>? Rooms { get; set; }
    }
}
