using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RoomMapper
    {
        private readonly RoomTypeMapper _roomTypeMapper;
        public RoomMapper()
        {
            _roomTypeMapper = new RoomTypeMapper();
        }

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
                LastUpdatedDateUtc = DateTime.Now,
            };
        }

        public RoomResponseModel MapToRoomResponseModel(RoomEntity entity)
        {
            return new RoomResponseModel
            {
                Id = entity.Id,
                FloorNumber = entity.FloorNumber,
                RoomNumber = entity.RoomNumber,
                RoomTypeId = entity.RoomTypeId,
                RoomTypeName = entity.RoomType?.RoomTypeName ?? string.Empty,
                Status = entity.Status.ToString(),
                TotalAvailableBed = entity.TotalAvailableBed,
                TotalUsedBed = entity.TotalUsedBed,
                Price = entity.RoomType?.Price ?? 0,
                BuildingId = entity.BuildingId,
                BuildingName = entity.Building?.Name ?? string.Empty,
                CreatedDateUtc = entity.CreatedDateUtc,
                CreatedBy = entity.CreatedBy,
                IsDeleted = entity.IsDeleted,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                RoomServices = entity.RoomType != null ? _roomTypeMapper.MapToRoomServiceModels(entity.RoomType) : [],
            };
        }
    }
}
