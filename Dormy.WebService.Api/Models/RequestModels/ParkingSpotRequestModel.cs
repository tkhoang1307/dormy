namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ParkingSpotRequestModel
    {
        public string ParkingSpotName { get; set; } = string.Empty;

        public int CapacitySpots { get; set; }
    }

    public class ParkingSpotUpdationRequestModel : ParkingSpotRequestModel
    {
        public Guid Id { get; set; }
    }
}
