using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class ServiceIndicatorMapper
    {
        public ServiceIndicatorEntity MapToServiceIndicatorEntity(ServiceIndicatorRequestModel model)
        {
            return new ServiceIndicatorEntity
            {
                RoomServiceId = model.RoomServiceId,
                RoomId = model.RoomId,
                Month = model.Month,
                Year = model.Year,
                //OldIndicator = model.OldIndicator,
                NewIndicator = model.NewIndicator,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public ServiceIndicatorResponseModel MapToServiceIndicatorResponseModel(ServiceIndicatorEntity entity)
        {
            return new ServiceIndicatorResponseModel
            {
                Id = entity.Id,
                RoomServiceId = entity.RoomServiceId,
                RoomId = entity.RoomId,
                RoomServiceName = entity.RoomService.RoomServiceName,
                RoomName = entity.Room.RoomNumber.ToString(),
                Month = entity.Month,
                Year = entity.Year,
                OldIndicator = entity.OldIndicator,
                NewIndicator = entity.NewIndicator,
                CreatedDateUtc = entity.CreatedDateUtc,
                CreatedBy = entity.CreatedBy,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                IsDeleted = entity.IsDeleted,
            };
        }
    }
}
