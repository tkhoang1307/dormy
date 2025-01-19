using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RoomMapper
    {
        public RoomEntity MapToRoomEntity(RoomRequestModel model, int floorNumber, Guid buildingId)
        {
            return new RoomEntity
            {
                Id = Guid.NewGuid(),
                FloorNumber = floorNumber,
                RoomNumer = model.RoomNumber,
                RoomTypeId = model.RoomTypeId,
                Status = model.RoomStatus,
                TotalAvailableBed = model.TotalAvailableBed,
                BuildingId = buildingId,
            };
        }

        public RoomResponseModel MapToRoomResponseModel(RoomEntity entity)
        {
            return new RoomResponseModel
            {
                Id = entity.Id,
                FloorNumber = entity.FloorNumber,
                RoomNumer = entity.RoomNumer,
                RoomTypeId = entity.RoomTypeId,
                RoomTypeName = entity.RoomType.RoomTypeName,
                Status = entity.Status,
                StatusName = entity.Status.ToString(),
                TotalAvailableBed = entity.TotalAvailableBed,
                BuildingId = entity.BuildingId,
            };
        }
    }
}
