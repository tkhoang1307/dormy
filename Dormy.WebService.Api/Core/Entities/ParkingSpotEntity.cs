using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class ParkingSpotEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ParkingSpotName { get; set; } = string.Empty;

        public int CapacitySpots { get; set; }

        public int CurrentQuantity { get; set; } = 0;

        public ParkingSpotStatusEnum Status { get; set; } = ParkingSpotStatusEnum.AVAILABLE;

        public List<ParkingRequestEntity>? ParkingRequests { get; set; }

        public List<VehicleHistoryEntity>? VehicleHistories { get; set; }
    }
}
