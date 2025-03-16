using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class VehicleResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string LicensePlate { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;

        public Guid? ParkingSpotId { get; set; }

        public string ParkingSpotName { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public string UserFullname { get; set; } = string.Empty;
    }

    public class VehicleHistoryResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string Action { get; set; } = string.Empty;

        public Guid VehicleId { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;

        public Guid ParkingSpotId { get; set; }

        public string ParkingSpotName { get; set; } = string.Empty;
    }
}
