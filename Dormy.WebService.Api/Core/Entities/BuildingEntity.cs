namespace Dormy.WebService.Api.Core.Entities
{
    public class BuildingEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        //Should define enum for this field
        public string GenderRestriction { get; set; } = string.Empty;

        public List<RoomEntity>? Rooms { get; set; }
    }
}
