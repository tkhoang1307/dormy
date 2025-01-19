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
            var buildingId = Guid.NewGuid();
            var buildingEntity = new BuildingEntity
            {
                Id = buildingId,
                CreatedDateUtc = DateTime.UtcNow,
                GenderRestriction = model.GenderRestriction,
                Name = model.Name,
                TotalFloors = model.TotalFloors,
                Rooms = []
            };

            foreach (var floor in model.Floors)
            {
                buildingEntity.Rooms.AddRange(floor.Rooms.Select(x => _roomMapper.MapToRoomEntity(x, floor.FloorId, buildingId)).ToList());
            }

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
                Floors = entity != null ? MapToFloorsResponseModel(entity) : [],
                isDeleted = entity != null && entity.isDeleted,
            };

            return buildingResponse;
        }

        public List<FloorResponseModel> MapToFloorsResponseModel(BuildingEntity building)
        {
            if(building.Rooms == null)
            {
                return [];
            }

            var roomsByFloor = GetRoomsByFloorId(building.Rooms);

            var listFloorResponse = new List<FloorResponseModel>();

            foreach (var floor in roomsByFloor)
            {
                var floorResponse = new FloorResponseModel
                {
                    FloorId = floor.Key,
                    Rooms = floor.Value.Select(x => _roomMapper.MapToRoomResponseModel(x)).ToList(),
                };

                listFloorResponse.Add(floorResponse);
            }

            return listFloorResponse;
        }

        private Dictionary<int, List<RoomEntity>> GetRoomsByFloorId(List<RoomEntity> rooms)
        {
            var floorRoomsDictionary = new Dictionary<int, List<RoomEntity>>();

            foreach (var room in rooms)
            {
                if (!floorRoomsDictionary.ContainsKey(room.FloorNumber))
                {
                    floorRoomsDictionary[room.FloorNumber] = new List<RoomEntity>();
                }
                floorRoomsDictionary[room.FloorNumber].Add(room);
            }

            return floorRoomsDictionary;
        }
    }
}
