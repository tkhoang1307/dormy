using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
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

        private async Task VerifyRoomCapacity(BuildingCreationRequestModel model)
        {
            // Get all unique RoomTypeIds from the model
            var roomTypeIds = model.Rooms.Select(room => room.RoomTypeId).Distinct().ToList();

            // Fetch all RoomType entities in a single query
            var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(rt => roomTypeIds.Contains(rt.Id));

            // Create a dictionary for quick lookup
            var roomTypeDict = roomTypes.ToDictionary(rt => rt.Id, rt => rt);

            // Validate each room in the model
            foreach (var room in model.Rooms)
            {
                if (!roomTypeDict.TryGetValue(room.RoomTypeId, out var roomType))
                {
                    throw new BadRequestException($"Room Type with ID: {room.RoomTypeId} was not found");
                }
            }
        }

        public async Task<ApiResponse> CreateBuilding(BuildingCreationRequestModel model)
        {
            await VerifyRoomCapacity(model);

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
                    include: building => building.Include(building => building.Rooms).ThenInclude(room => room.RoomType).ThenInclude(roomType => roomType.RoomTypeServices).ThenInclude(roomTypeService => roomTypeService.RoomService)
                );

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }
            var response = _buildingMapper.MapToBuildingResponseModel(entity);

            var author = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(entity.CreatedBy));

            response.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(author);

            if (entity.LastUpdatedBy != null)
            {
                var updatedAdmin = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(entity.LastUpdatedBy));
                response.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(updatedAdmin);
            }

            for (int i = 0; i < response.Rooms.Count; i++)
            {
                var roomResponseModel = response.Rooms[i];

                var (createRoomAuthor, lastUpdateRoomAuthor) = await _unitOfWork.AdminRepository.GetAuthors(roomResponseModel.CreatedBy, roomResponseModel.LastUpdatedBy);

                roomResponseModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createRoomAuthor);
                roomResponseModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdateRoomAuthor);
            }

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetBuildingBatch(GetBatchRequestModel request)
        {
            var entities = await _unitOfWork.BuildingRepository
                .GetAllAsync(
                    building => true,
                    include: building => building.Include(building => building.Rooms)
                                                    .ThenInclude(room => room.RoomType)
                                                    .ThenInclude(roomType => roomType.RoomTypeServices)
                                                    .ThenInclude(roomTypeService => roomTypeService.RoomService)
                );

            if (request.Ids.Count > 0)
            {
                entities = entities.Where(x => request.Ids.Contains(x.Id)).ToList();
            }

            var buildingListResponseModel = entities.Select(entity => _buildingMapper.MapToBuildingBatchResponseModel(entity)).ToList();


            for (int i = 0; i < buildingListResponseModel.Count; i++)
            {
                var buildingResponseModel = buildingListResponseModel[i];

                var (createAuthor, lastUpdateAuthor) = await _unitOfWork.AdminRepository.GetAuthors(buildingResponseModel.CreatedBy, buildingResponseModel.LastUpdatedBy);

                buildingResponseModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createAuthor);
                buildingResponseModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdateAuthor);
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

        public async Task<ApiResponse> UpdateBuilding(BuildingUpdationRequestModel model)
        {
            var buildingEntity = await _unitOfWork.BuildingRepository.GetAsync(x => x.Id.Equals(model.Id));

            if (buildingEntity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Building"));
            }

            buildingEntity.Name = model.Name;
            buildingEntity.TotalFloors = model.TotalFloors;
            buildingEntity.GenderRestriction = (GenderEnum)Enum.Parse(typeof(GenderEnum), model.GenderRestriction);
            buildingEntity.LastUpdatedBy = _userContextService.UserId;
            buildingEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(buildingEntity.Id);
        }
    }
}
