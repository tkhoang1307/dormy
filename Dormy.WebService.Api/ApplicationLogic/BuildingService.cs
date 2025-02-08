using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class BuildingService : IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly BuildingMapper _buildingMapper;

        public BuildingService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _buildingMapper = new BuildingMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> CreateBuilding(BuildingCreationRequestModel model)
        {
            var roomTypeIds = model.Rooms
                       .Select(room => room.RoomTypeId)
                       .Distinct()
                       .ToList();

            var roomTypes = await _unitOfWork.RoomTypeRepository
                                             .GetAllAsync(rt => roomTypeIds.Contains(rt.Id));

            if (roomTypes.Count != roomTypeIds.Count)
            {
                var missingIds = roomTypeIds.Except(roomTypes.Select(rt => rt.Id));
                return new ApiResponse().SetBadRequest($"Room Types with IDs: {string.Join(", ", missingIds)} were not found");
            }

            var roomsRequestModel = new List<RoomRequestModel>();

            for (int iRoom = 0; iRoom < model.Rooms.Count; iRoom++)
            {
                var room = model.Rooms[iRoom];
                int totalRoomsCreated = RoomHelper.CalculateTotalRoomsWereCreatedBeforeInARequest(model.Rooms, iRoom);

                int maxRoomNumberOnFloor = 0;
                int roomNumberStartToMark = RoomHelper.CalculateStartedRoomNumberInARequest(room.FloorNumber, maxRoomNumberOnFloor, totalRoomsCreated);

                for (var i = 0; i < room.TotalRoomsWantToCreate; i++)
                {
                    roomsRequestModel.Add(new RoomRequestModel()
                    {
                        RoomStatus = RoomStatusEnum.AVAILABLE,
                        RoomTypeId = room.RoomTypeId,
                        FloorNumber = room.FloorNumber,
                        RoomNumber = roomNumberStartToMark,
                    });

                    roomNumberStartToMark = roomNumberStartToMark + 1;
                }
            }
            var buildingRequestModel = new BuildingRequestModel()
            {
                Name = model.Name,
                TotalFloors = model.TotalFloors,
                GenderRestriction = model.GenderRestriction,
                Rooms = roomsRequestModel,
            };
            var buildingEntity = _buildingMapper.MapToBuildingEntity(buildingRequestModel);

            if (_userContextService.UserId != Guid.Empty)
            {
                buildingEntity.CreatedBy = _userContextService.UserId;
                buildingEntity.LastUpdatedBy = _userContextService.UserId;
            }

            if (buildingEntity.Rooms != null && buildingEntity.Rooms.Count > 0)
            {
                foreach (var room in buildingEntity.Rooms)
                {
                    room.CreatedBy = _userContextService.UserId;
                    room.LastUpdatedBy = _userContextService.UserId;
                    room.TotalAvailableBed = roomTypes.FirstOrDefault(t => t.Id == room.RoomTypeId)?.Capacity ?? room.TotalAvailableBed; 
                }
            }

            await _unitOfWork.BuildingRepository.AddAsync(buildingEntity);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(buildingEntity.Id);
        }

        public async Task<ApiResponse> CreateBuildingBatch(List<BuildingCreationRequestModel> models)
        {
            var roomTypeIds = models
                       .SelectMany(b => b.Rooms)
                       .Select(room => room.RoomTypeId)
                       .Distinct()
                       .ToList();

            var roomTypes = await _unitOfWork.RoomTypeRepository
                                             .GetAllAsync(rt => roomTypeIds.Contains(rt.Id));

            if (roomTypes.Count != roomTypeIds.Count)
            {
                var missingIds = roomTypeIds.Except(roomTypes.Select(rt => rt.Id));
                return new ApiResponse().SetBadRequest($"Room Types with IDs: {string.Join(", ", missingIds)} were not found");
            }

            var listBuildingEntities = new List<BuildingEntity>();

            foreach (var model in models)
            {
                var roomsRequestModel = new List<RoomRequestModel>();

                for (int iRoom = 0; iRoom < model.Rooms.Count; iRoom++)
                {
                    var room = model.Rooms[iRoom];
                    int totalRoomsCreated = RoomHelper.CalculateTotalRoomsWereCreatedBeforeInARequest(model.Rooms, iRoom);

                    int maxRoomNumberOnFloor = 0;
                    int roomNumberStartToMark = RoomHelper.CalculateStartedRoomNumberInARequest(room.FloorNumber, maxRoomNumberOnFloor, totalRoomsCreated);

                    for (var i = 0; i < room.TotalRoomsWantToCreate; i++)
                    {
                        roomsRequestModel.Add(new RoomRequestModel()
                        {
                            RoomStatus = RoomStatusEnum.AVAILABLE,
                            RoomTypeId = room.RoomTypeId,
                            FloorNumber = room.FloorNumber,
                            RoomNumber = roomNumberStartToMark,
                        });

                        roomNumberStartToMark = roomNumberStartToMark + 1;
                    }
                }
                var buildingRequestModel = new BuildingRequestModel()
                {
                    Name = model.Name,
                    TotalFloors = model.TotalFloors,
                    GenderRestriction = model.GenderRestriction,
                    Rooms = roomsRequestModel,
                };
                var buildingEntity = _buildingMapper.MapToBuildingEntity(buildingRequestModel);

                if (_userContextService.UserId != Guid.Empty)
                {
                    buildingEntity.CreatedBy = _userContextService.UserId;
                    buildingEntity.LastUpdatedBy = _userContextService.UserId;
                }

                if (buildingEntity.Rooms != null && buildingEntity.Rooms.Count > 0)
                {
                    foreach (var room in buildingEntity.Rooms)
                    {
                        room.CreatedBy = _userContextService.UserId;
                        room.LastUpdatedBy = _userContextService.UserId;
                        room.TotalAvailableBed = roomTypes.FirstOrDefault(t => t.Id == room.RoomTypeId)?.Capacity ?? room.TotalAvailableBed;
                    }
                }

                listBuildingEntities.Add(buildingEntity);
            }

            await _unitOfWork.BuildingRepository.AddRangeAsync(listBuildingEntities);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(listBuildingEntities.Select(b => b.Id).ToList());
        }

        public async Task<ApiResponse> GetBuildingById(Guid id)
        {
            var entity = await _unitOfWork.BuildingRepository
                .GetAsync(
                    building => building.Id.Equals(id),
                    include: building => building.Include(building => building.Rooms).ThenInclude(room => room.RoomType)
                );

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }
            var response = _buildingMapper.MapToBuildingResponseModel(entity);

            var author = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(entity.CreatedBy));

            response.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(author);

            if (entity.LastUpdatedBy != null)
            {
                var updatedAdmin = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(entity.LastUpdatedBy));
                response.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(updatedAdmin);
            }

            for (int i = 0; i < response.Rooms.Count; i++)
            {
                var roomResponseModel = response.Rooms[i];

                var (createRoomAuthor, lastUpdateRoomAuthor) = await _unitOfWork.AdminRepository.GetAuthors(roomResponseModel.CreatedBy, roomResponseModel.LastUpdatedBy);

                roomResponseModel.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(createRoomAuthor);
                roomResponseModel.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(lastUpdateRoomAuthor);
            }

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetBuildingBatch(List<Guid> ids, bool isGetAll = false)
        {
            var entities = await _unitOfWork.BuildingRepository
                .GetAllAsync(
                    building => isGetAll || ids.Contains(building.Id),
                    include: building => building.Include(building => building.Rooms).ThenInclude(room => room.RoomType)
                );

            if (entities == null)
            {
                return new ApiResponse().SetOk(new());
            }
            var buildingListResponseModel = entities.Select(entity => _buildingMapper.MapToBuildingResponseModel(entity)).ToList();


            for (int i = 0; i < buildingListResponseModel.Count; i++)
            {
                var buildingResponseModel = buildingListResponseModel[i];

                var (createAuthor, lastUpdateAuthor) = await _unitOfWork.AdminRepository.GetAuthors(buildingResponseModel.CreatedBy, buildingResponseModel.LastUpdatedBy);

                buildingResponseModel.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(createAuthor);
                buildingResponseModel.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(lastUpdateAuthor);

                for (int j = 0; j < buildingResponseModel.Rooms.Count; j++)
                {
                    var roomResponseModel = buildingResponseModel.Rooms[j];

                    var (createRoomAuthor, lastUpdateRoomAuthor) = await _unitOfWork.AdminRepository.GetAuthors(roomResponseModel.CreatedBy, roomResponseModel.LastUpdatedBy);

                    roomResponseModel.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(createRoomAuthor);
                    roomResponseModel.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(lastUpdateRoomAuthor);
                }
            }

            return new ApiResponse().SetOk(buildingListResponseModel);
        }

        public async Task<ApiResponse> SoftDeleteBuildingById(Guid id)
        {
            var buildingEntity = await _unitOfWork.BuildingRepository.GetAsync(x => x.Id.Equals(id), x => x.Include(x => x.Rooms));

            if (buildingEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Building"));
            }

            if (buildingEntity.Rooms != null && buildingEntity.Rooms.Count > 0)
            {
                if (buildingEntity.Rooms.Any(r => r.TotalUsedBed > 0))
                {
                    return new ApiResponse().SetBadRequest(ErrorMessages.RoomIsOccupiedErrorMessage);
                }
            }

            buildingEntity.IsDeleted = true;
            buildingEntity.LastUpdatedDateUtc = DateTime.UtcNow;
            buildingEntity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk(buildingEntity.Id);
        }
    }
}
