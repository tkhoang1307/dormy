﻿using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class VehicleRequestModel
    {
        public string LicensePlate { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;

        public Guid? UserId { get; set; }
    }

    public class VehicleUpdationRequestModel
    {
        public Guid Id { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;
    }

    public class VehicleHistoryRequestModel
    {
        public bool IsIn { get; set; }
        public Guid VehicleId { get; set; }
        public Guid ParkingSpotId { get; set; }
    }
}
