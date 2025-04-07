using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ParkingRequestResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public string UserFullName { get; set; } = string.Empty;

        public Guid ParkingSpotId { get; set; }

        public string ParkingSpotName { get; set; } = string.Empty;

        public string ParkingSpotStatus { get; set; } = string.Empty;

        public Guid VehicleId { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;

        public Guid? ApproverId { get; set; }

        public string ApproverUserFullName { get; set; } = string.Empty;
    }
}
