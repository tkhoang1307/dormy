using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class VehicleHistoryMapper
    {
        public VehicleHistoryEntity MapToVehicleHistoryEntity(VehicleHistoryRequestModel model)
        {
            return new VehicleHistoryEntity
            {
                VehicleId = model.VehicleId,
                ParkingSpotId = model.ParkingSpotId,
                Action = model.IsIn ? VehicleActionEnum.IN : VehicleActionEnum.OUT,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public VehicleHistoryResponseModel MapToVehicleHistoryResponseModel(VehicleHistoryEntity entity)
        {
            return new VehicleHistoryResponseModel
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                ParkingSpotId = entity.ParkingSpotId,
                Action = entity.Action.ToString(),
                LicensePlate = entity.Vehicle.LicensePlate,
                VehicleType = entity.Vehicle.VehicleType,
                ParkingSpotName = entity.ParkingSpot.ParkingSpotName,
                CreatedBy = entity.CreatedBy, 
                LastUpdatedBy = entity.LastUpdatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                IsDeleted = entity.IsDeleted,
            };
        }
    }
}
