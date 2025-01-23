using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RoomMapper
    {
        public RoomEntity MapToRoomEntity(RoomRequestModel model)
        {
            return new RoomEntity
            {
                FloorNumber = model.FloorNumber,
                RoomNumber = model.RoomNumber,
                RoomTypeId = model.RoomTypeId,
                Status = model.RoomStatus,
                TotalAvailableBed = model.TotalAvailableBed,
                CreatedDateUtc = DateTime.Now,
            };
        }

        public RoomResponseModel MapToRoomResponseModel(RoomEntity entity)
        {
            return new RoomResponseModel
            {
                Id = entity.Id,
                FloorNumber = entity.FloorNumber,
                RoomNumer = entity.RoomNumber,
                RoomTypeId = entity.RoomTypeId,
                RoomTypeName = entity.RoomType?.RoomTypeName ?? string.Empty,
                Status = entity.Status,
                StatusName = entity.Status.ToString(),
                TotalAvailableBed = entity.TotalAvailableBed,
                BuildingId = entity.BuildingId,
                CreatedDateUtc = entity.CreatedDateUtc,
                CreatedBy = entity.CreatedBy,
                isDeleted = entity.isDeleted,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
            };
        }
    }
}
