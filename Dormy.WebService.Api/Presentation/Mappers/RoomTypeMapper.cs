using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RoomTypeMapper
    {
        public RoomTypeEntity MapToRoomTypeEnity(RoomTypeRequestModel model)
        {
            return new RoomTypeEntity
            {
                Id = Guid.NewGuid(),
                Capacity = model.Capacity,
                CreatedDateUtc = DateTime.UtcNow,
                Description = model.Description,
                Price = model.Price,
                RoomTypeName = model.RoomTypeName,
            };
        }

        public RoomTypeResponseModel MapToRoomTypeResponseModel(RoomTypeEntity entity)
        {
            return new RoomTypeResponseModel
            {
                Id = entity.Id,
                Capacity = entity.Capacity,
                CreatedDateUtc = entity.CreatedDateUtc,
                Description = entity.Description,
                Price = entity.Price,
                RoomTypeName = entity.RoomTypeName,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                CreatedBy = entity.CreatedBy,
                isDeleted = entity.isDeleted
            };
        }
    }
}
