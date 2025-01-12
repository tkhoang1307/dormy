using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class VehicleHistoryEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public VehicleActionEnum Action { get; set; }
        
        public Guid VehicleId { get; set; }

        public VehicleEntity Vehicle { get; set; } = null!;

        public Guid ParkingSpotId { get; set; }

        public ParkingSpotEntity ParkingSpot { get; set; } = null!;
    }
}
