using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class ParkingRequestMapper
    {
        public ParkingRequestEntity MapToParkingRequestEntity(ParkingRequestModel model)
        {
            return new ParkingRequestEntity
            {
                ParkingSpotId = model.ParkingSpotId,
                Status = RequestStatusEnum.SUBMITTED,
                VehicleId = model.VehicleId,
                Description = model.Description,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public ParkingRequestResponseModel MapToParkingRequestResponseModel(ParkingRequestEntity entity)
        {
            return new ParkingRequestResponseModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Status = entity.Status.ToString(),
                UserId = entity.UserId,
                VehicleId = entity.VehicleId,
                ParkingSpotId = entity.ParkingSpotId,
                ApproverId = entity.ApproverId,
                ApproverUserFullName = entity.Approver != null ? entity.Approver.LastName + " " + entity.Approver.FirstName : string.Empty,
                LicensePlate = entity.Vehicle.LicensePlate,
                VehicleType = entity.Vehicle.VehicleType,
                ParkingSpotName = entity.ParkingSpot.ParkingSpotName,
                ParkingSpotStatus = entity.ParkingSpot.Status.ToString(),
                UserFullName = entity.User.LastName + " " + entity.User.FirstName,
                CreatedBy = entity.CreatedBy,
                LastUpdatedBy = entity.LastUpdatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                IsDeleted = entity.IsDeleted,
            };
        }
    }
}
