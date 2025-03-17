using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Presentation.Validations;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly RoomMapper _roomMapper;

        public RoomService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _roomMapper = new RoomMapper();
        }

        public async Task<ApiResponse> CreateRoomBatch(List<RoomCreationRequestModel> rooms, Guid buildingId)
        {
            var buildingEntity = await _unitOfWork.BuildingRepository.GetAsync(x => x.Id.Equals(buildingId), x => x.Include(x => x.Rooms));

            if (buildingEntity == null)
            {
                return new ApiResponse().SetNotFound(buildingId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Building"));
            }

            var roomTypeIds = rooms.Select(x => x.RoomTypeId).Distinct().ToList();

            var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => roomTypeIds.Contains(x.Id));

            if (roomTypes == null || (roomTypes != null && roomTypes.Count != roomTypeIds.Count))
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.SomeRoomTypesAreNotExisted);
            }

            var entities = new List<RoomEntity>();

            for (int iRoom = 0; iRoom < rooms.Count; iRoom++)
            {
                var room = rooms[iRoom];
                int totalRoomsCreated = RoomHelper.CalculateTotalRoomsWereCreatedBeforeInARequest(rooms, iRoom);
                
                int maxRoomNumberOnFloor = buildingEntity.Rooms
                                                         .Where(r => r.FloorNumber == room.FloorNumber)
                                                         .Max(r => (int?)r.RoomNumber) ?? 0;
                int roomNumberStartToMark = RoomHelper.CalculateStartedRoomNumberInARequest(room.FloorNumber, maxRoomNumberOnFloor, totalRoomsCreated);
                
                for (var i = 0; i < room.TotalRoomsWantToCreate; i++)
                {
                    var entity = _roomMapper.MapToRoomEntity(new RoomRequestModel()
                    {
                        RoomStatus = RoomStatusEnum.AVAILABLE,
                        RoomTypeId = room.RoomTypeId,
                        FloorNumber = room.FloorNumber,
                    });
                    entity.BuildingId = buildingId;
                    entity.CreatedBy = _userContextService.UserId;
                    entity.LastUpdatedBy = _userContextService.UserId;
                    entity.TotalAvailableBed = roomTypes.FirstOrDefault(t => t.Id == entity.RoomTypeId)?.Capacity ?? entity.TotalAvailableBed;
                    entity.RoomNumber = roomNumberStartToMark;

                    entities.Add(entity);

                    roomNumberStartToMark = roomNumberStartToMark + 1;
                }    
            }

            await _unitOfWork.RoomRepository.AddRangeAsync(entities);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(buildingId);
        }

        public async Task<ApiResponse> GetRoomById(Guid id)
        {
            var entity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(id), x => x.Include(x => x.RoomType).ThenInclude(roomType => roomType.RoomTypeServices).ThenInclude(roomTypeService => roomTypeService.RoomService));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            var response = _roomMapper.MapToRoomResponseModel(entity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(response.CreatedBy, response.LastUpdatedBy);

            response.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
            response.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

            foreach(var roomServiceModel in response.RoomServices)
            {
                var (createdUserRoomService, lastUpdatedUserRoomService) = await _unitOfWork.AdminRepository.GetAuthors(roomServiceModel.CreatedBy, roomServiceModel.LastUpdatedBy);

                roomServiceModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUserRoomService);
                roomServiceModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUserRoomService);
            }    

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetRoomsByBuildingId(Guid buildingId)
        {
            var entity = await _unitOfWork.BuildingRepository.GetAsync(x => x.Id.Equals(buildingId), x => x.Include(x => x.Rooms).ThenInclude(x => x.RoomType));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(buildingId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Building"));
            }

            entity.Rooms ??= [];

            var response = entity.Rooms.Select(r => _roomMapper.MapToRoomBatchResponseModel(r)).OrderBy(r => r.RoomNumber).ToList();
            foreach(var roomModel in response)
            {
                var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(roomModel.CreatedBy, roomModel.LastUpdatedBy);

                roomModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
                roomModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);
            }

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> SoftDeleteRoom(Guid id)
        {
            var entity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(id));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            if (entity.TotalUsedBed > 0)
            {
                return new ApiResponse().SetBadRequest(entity.Id, ErrorMessages.RoomIsOccupiedErrorMessage);
            }

            entity.IsDeleted = true;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(id);
        }

        public async Task<ApiResponse> SoftDeleteRoomBatch(List<Guid> ids)
        {
            var entities = await _unitOfWork.RoomRepository.GetAllAsync(x => ids.Contains(x.Id));

            if (entities == null || (entities.Count != ids.Count))
            {
                return new ApiResponse().SetNotFound(message: ErrorMessages.SomeRoomTypesAreNotExisted);
            }

            if (entities.Any(r => r.TotalUsedBed > 0))
            {
                return new ApiResponse().SetBadRequest("There are some beds is in used.");
            }

            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.LastUpdatedDateUtc = DateTime.UtcNow;
                entity.LastUpdatedBy = _userContextService.UserId;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> UpdateRoom(RoomUpdateRequestModel model)
        {
            var entity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.Id));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            var roomType = await _unitOfWork.RoomTypeRepository.GetAsync(x => x.Id.Equals(model.RoomTypeId));

            if (roomType == null)
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.SomeRoomTypesAreNotExisted);
            }

            if (entity.RoomTypeId != model.RoomTypeId)
            {
                if (entity.TotalUsedBed < roomType.Capacity)
                {
                    entity.Status = RoomStatusEnum.AVAILABLE;
                }
                else
                {
                    entity.Status = RoomStatusEnum.FULL;
                }
                entity.TotalAvailableBed = roomType.Capacity;
            }

            if (entity.FloorNumber != model.FloorNumber)
            {
                entity.FloorNumber = model.FloorNumber;

                var buildingEntity = await _unitOfWork.BuildingRepository.GetAsync(x => x.Id.Equals(entity.BuildingId), x => x.Include(x => x.Rooms));
                int roomCountOnFloor = buildingEntity.Rooms.Count(r => r.FloorNumber == entity.FloorNumber);
                entity.RoomNumber = entity.FloorNumber * 100 + roomCountOnFloor + 1;
            }

            entity.RoomTypeId = model.RoomTypeId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(model.Id);
        }

        public async Task<ApiResponse> UpdateRoomStatus(RoomUpdateStatusRequestModel model)
        {
            var entity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.Id));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            entity.Status = (RoomStatusEnum)Enum.Parse(typeof(RoomStatusEnum), model.Status);
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(model.Id);
        }

        public async Task<List<Guid>> GetAllUsersOfRoomByRoomId(Guid roomId)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(roomId));
            if (roomEntity == null)
            {
                return [];
            }

            var entityContracts = await _unitOfWork.ContractRepository
                                                 .GetAllAsync(x => x.RoomId.Equals(roomId)
                                                              && (x.Status == ContractStatusEnum.ACTIVE || x.Status == ContractStatusEnum.EXTENDED));
            
            var userIds = entityContracts.Select(c => c.UserId).ToList();

            return userIds;
        }

        public async Task<List<Guid>> GetAllRoomServicesOfRoomByRoomId(Guid roomId)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(roomId));
            if (roomEntity == null)
            {
                return [];
            }

            var roomTypeId = roomEntity.RoomTypeId;
            var roomTypeServiceEntities = await _unitOfWork.RoomTypeServiceRepository.GetAllAsync(x => x.RoomTypeId.Equals(roomTypeId));
            var roomServiceIds = roomTypeServiceEntities.Select(rts => rts.RoomServiceId).ToList();

            return roomServiceIds;
        }
    }
}
