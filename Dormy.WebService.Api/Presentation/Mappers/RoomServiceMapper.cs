using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RoomServiceMapper
    {
        public RoomServiceEntity MapToRoomServiceEntity(RoomServiceRequestModel model)
        {
            return new RoomServiceEntity
            {
                Cost = model.Cost,
                Unit = model.Unit,
                RoomServiceName = model.RoomServiceName,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public RoomServiceResponseModel MapToRoomServiceModel(RoomServiceEntity model)
        {
            return new RoomServiceResponseModel
            {
                Id = model.Id,
                Cost = model.Cost,
                Unit = model.Unit,
                RoomServiceName = model.RoomServiceName,
                CreatedDateUtc = model.CreatedDateUtc,
                CreatedBy = model.CreatedBy,
                LastUpdatedBy = model.LastUpdatedBy,
                LastUpdatedDateUtc = model.LastUpdatedDateUtc,
                IsDeleted = model.IsDeleted,
            };
        }
    }
}
