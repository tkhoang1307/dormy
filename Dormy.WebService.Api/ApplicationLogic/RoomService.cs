using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
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

        public async Task<ApiResponse> CreateRoomBatch(List<RoomRequestModel> rooms, Guid buildingId)
        {
            var buildingEntity = await _unitOfWork.BuildingRepository.GetAsync(x => x.Id.Equals(buildingId));

            if (buildingEntity == null)
            {
                return new ApiResponse().SetNotFound(buildingId);
            }

            var roomTypeIds = rooms.Select(x => x.RoomTypeId).Distinct().ToList();

            var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => roomTypeIds.Contains(x.Id));

            if (roomTypes == null || (roomTypes != null && roomTypes.Count != roomTypeIds.Count))
            {
                return new ApiResponse().SetBadRequest("Some of room type is not found");
            }

            var entities = rooms.Select(r => _roomMapper.MapToRoomEntity(r)).ToList();

            foreach (var entity in entities)
            {
                entity.BuildingId = buildingId;
                entity.CreatedBy = _userContextService.UserId;
            }

            await _unitOfWork.RoomRepository.AddRangeAsync(entities);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(buildingId);
        }

        public async Task<ApiResponse> GetRoomById(Guid id)
        {
            var entity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(id), x => x.Include(x => x.RoomType));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            var response = _roomMapper.MapToRoomResponseModel(entity);
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetRoomsByBuildingId(Guid buildingId)
        {
            var entity = await _unitOfWork.BuildingRepository.GetAsync(x => x.Id.Equals(buildingId), x => x.Include(x => x.Rooms).ThenInclude(x => x.RoomType));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(buildingId);
            }

            entity.Rooms ??= [];

            var response = entity.Rooms.Select(r => _roomMapper.MapToRoomResponseModel(r)).OrderBy(r => r.RoomNumer).ToList();
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> SoftDeleteRoom(Guid id)
        {
            var entity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(id));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            if (BedHelper.IsBedOccupied([entity]))
            {
                return new ApiResponse().SetBadRequest(entity.Id, ErrorMessages.BedIsOccupiedErrorMessage);
            }

            entity.isDeleted = true;
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
                return new ApiResponse().SetNotFound("Some of room is not found");
            }

            if (BedHelper.IsBedOccupied(entities))
            {
                return new ApiResponse().SetBadRequest("There are some beds is in used.");
            }

            foreach (var entity in entities)
            {
                entity.isDeleted = true;
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
                return new ApiResponse().SetNotFound(model.Id);
            }

            var roomType = await _unitOfWork.RoomTypeRepository.GetAsync(x => x.Id.Equals(model.RoomTypeId));

            if (roomType == null)
            {
                return new ApiResponse().SetBadRequest("Some of room type is not found");
            }

            entity.RoomNumber = model.RoomNumber;
            entity.FloorNumber = model.FloorNumber;
            entity.Status = model.RoomStatus;
            entity.RoomTypeId = model.RoomTypeId;
            entity.TotalAvailableBed = model.TotalAvailableBed;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(model.Id);
        }

        public async Task<ApiResponse> UpdateRoomStatus(RoomUpdateStatusRequestModel model)
        {
            var entity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.Id));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id);
            }

            entity.Status = model.Status;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(model.Id);
        }
    }
}
