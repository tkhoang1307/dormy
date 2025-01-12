using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class BuildingEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        public GenderEnum GenderRestriction { get; set; }

        public List<RoomEntity>? Rooms { get; set; }
    }
}
