using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class VehicleMapper
    {
        public VehicleEntity MapToVehicleEntity(VehicleRequestModel model)
        {
            return new VehicleEntity
            {
                LicensePlate = model.LicensePlate,
                VehicleType = model.VehicleType,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public VehicleResponseModel MapToVehicleResponseModel(VehicleEntity model)
        {
            return new VehicleResponseModel
            {
                Id = model.Id,
                LicensePlate = model.LicensePlate,
                VehicleType = model.VehicleType,
                ParkingSpotId = model.ParkingSpotId,
                UserId = model.UserId,
                CreatedDateUtc = model.CreatedDateUtc,
                CreatedBy = model.CreatedBy,
                LastUpdatedBy = model.LastUpdatedBy,
                LastUpdatedDateUtc = model.LastUpdatedDateUtc,
                IsDeleted = model.IsDeleted,
            };
        }
    }
}
