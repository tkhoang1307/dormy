namespace Dormy.WebService.Api.Core.Entities
{
    public class VehicleEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string LicensePlate { get; set; } = string.Empty;
        
        public string VehicleType { get; set; } = string.Empty;

        public Guid? ParkingSpotId { get; set; }

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;

        public List<ParkingRequestEntity>? ParkingRequests { get; set; }

        public List<VehicleHistoryEntity>? VehicleHistories { get; set; }
    }
}
