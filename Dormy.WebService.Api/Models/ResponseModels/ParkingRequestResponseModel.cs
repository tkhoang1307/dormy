using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ParkingRequestResponseModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public RequestStatusEnum Status { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string UserFullName { get; set; } = string.Empty;

        public Guid ParkingSpotId { get; set; }

        public string ParkingSpotName { get; set; } = string.Empty;

        public ParkingSpotStatusEnum ParkingSpotStatus { get; set; }

        public Guid VehicleId { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;

        public Guid? ApproverId { get; set; }

        public string ApproverUserName { get; set; } = string.Empty;

        public string ApproverUserFullName { get; set; } = string.Empty;
    }
}
