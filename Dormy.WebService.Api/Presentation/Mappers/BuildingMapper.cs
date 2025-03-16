using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
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
                LastUpdatedDateUtc = DateTime.UtcNow,
                GenderRestriction = (GenderEnum)Enum.Parse(typeof(GenderEnum), model.GenderRestriction),
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
                GenderRestriction = entity.GenderRestriction.ToString(),
                Name = entity.Name,
                TotalFloors = entity.TotalFloors,
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                LastUpdatedBy = entity.LastUpdatedBy,
                TotalRooms = entity?.Rooms?.Count ?? 0,
                Rooms = (entity != null && entity.Rooms != null) ? entity.Rooms.Select(r => _roomMapper.MapToRoomBatchResponseModel(r)).OrderBy(r => r.FloorNumber).ToList() : [],
                IsDeleted = entity != null && entity.IsDeleted,
            };

            return buildingResponse;
        }

        public BuildingBatchResponseModel MapToBuildingBatchResponseModel(BuildingEntity entity)
        {
            var buildingResponse = new BuildingBatchResponseModel
            {
                Id = entity.Id,
                GenderRestriction = entity.GenderRestriction.ToString(),
                Name = entity.Name,
                TotalFloors = entity.TotalFloors,
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                LastUpdatedBy = entity.LastUpdatedBy,
                TotalRooms = entity?.Rooms?.Count ?? 0,
                IsDeleted = entity != null && entity.IsDeleted,
            };

            return buildingResponse;
        }
    }
}
