namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ParkingSpotResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ParkingSpotName { get; set; } = string.Empty;

        public int CapacitySpots { get; set; }

        public int CurrentQuantity { get; set; }

        // ParkingSpotStatusEnum
        public string Status { get; set; } = string.Empty;
    }
}
