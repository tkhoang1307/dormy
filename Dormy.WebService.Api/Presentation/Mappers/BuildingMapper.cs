using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class BuildingMapper
    {
        private readonly RoomMapper _roomMapper;

        public BuildingMapper()
        {
            _roomMapper = new RoomMapper();
        }

        public BuildingEntity MapToBuildingEntity(BuildingRequestModel model)
        {
            var buildingEntity = new BuildingEntity
            {
                CreatedDateUtc = DateTime.UtcNow,
                GenderRestriction = model.GenderRestriction,
                Name = model.Name,
                TotalFloors = model.TotalFloors,
                Rooms = model.Rooms.Select(r => _roomMapper.MapToRoomEntity(r)).ToList(),
            };

            return buildingEntity;
        }

        public BuildingResponseModel MapToBuildingResponseModel(BuildingEntity entity)
        {
            var buildingResponse = new BuildingResponseModel
            {
                Id = entity.Id,
                GenderRestriction = entity.GenderRestriction,
                Name = entity.Name,
                TotalFloors = entity.TotalFloors,
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                LastUpdatedBy = entity.LastUpdatedBy,
                TotalRooms = entity?.Rooms?.Count ?? 0,
                Rooms = (entity != null && entity.Rooms != null) ? entity.Rooms.Select(r => _roomMapper.MapToRoomResponseModel(r)).OrderBy(r => r.FloorNumber).ToList() : [],
                IsDeleted = entity != null && entity.IsDeleted,
            };

            return buildingResponse;
        }
    }
}
