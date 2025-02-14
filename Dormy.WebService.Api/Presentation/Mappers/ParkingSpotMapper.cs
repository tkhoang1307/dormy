using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class ParkingSpotMapper
    {
        public ParkingSpotEntity MapToParkingSpotEntity(ParkingSpotRequestModel model)
        {
            return new ParkingSpotEntity
            {
                ParkingSpotName = model.ParkingSpotName,
                CapacitySpots = model.CapacitySpots,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public ParkingSpotResponseModel MapToParkingSpotModel(ParkingSpotEntity model)
        {
            return new ParkingSpotResponseModel
            {
                Id = model.Id,
                ParkingSpotName = model.ParkingSpotName,
                CapacitySpots = model.CapacitySpots,
                CurrentQuantity = model.CurrentQuantity,
                Status = model.Status.ToString(),
                CreatedDateUtc = model.CreatedDateUtc,
                CreatedBy = model.CreatedBy,
                LastUpdatedBy = model.LastUpdatedBy,
                LastUpdatedDateUtc = model.LastUpdatedDateUtc,
                IsDeleted = model.IsDeleted,
            };
        }
    }
}
