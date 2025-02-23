using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ParkingRequestModel
    {
        public string Description { get; set; } = string.Empty;

        public Guid ParkingSpotId { get; set; }

        public Guid VehicleId { get; set; }
    }

    public class UpdateParkingRequestModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public Guid ParkingSpotId { get; set; }

    }

    public class ApproveOrRejectParkingRequestModel
    {
        public Guid Id { get; set; }
        public bool IsAccepted { get; set; }
    }

    public class ParkingRequestStatusModel
    {
        public Guid Id { get; set; }
        public RequestStatusEnum Status { get; set; }
    }
}
