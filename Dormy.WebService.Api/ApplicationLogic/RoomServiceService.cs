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
    public class RoomServiceService : IRoomServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private RoomServiceMapper _roomServiceMapper;

        public RoomServiceService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _roomServiceMapper = new RoomServiceMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> AddRoomService(RoomServiceRequestModel model)
        {
            var entity = _roomServiceMapper.MapToRoomServiceEntity(model);

            entity.CreatedBy = _userContextService.UserId;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.RoomServiceRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> AddRoomServiceBatch(List<RoomServiceRequestModel> models)
        {
            var entities = models.Select(x => _roomServiceMapper.MapToRoomServiceEntity(x)).ToList();

            foreach (var entity in entities)
            {
                entity.CreatedBy = _userContextService.UserId;
                entity.LastUpdatedBy = _userContextService.UserId;
            }

            await _unitOfWork.RoomServiceRepository.AddRangeAsync(entities);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entities.Select(x => x.Id).ToList());
        }

        public async Task<ApiResponse> GetRoomServiceBatch(GetBatchRequestModel model)
        {
            var entities = new List<RoomServiceEntity>();

            if (model.IsGetAll)
            {
                entities = await _unitOfWork.RoomServiceRepository.GetAllAsync(x => true);
            }
            else
            {
                entities = await _unitOfWork.RoomServiceRepository.GetAllAsync(x => model.Ids.Contains(x.Id));
            }

            var roomServiceModels = entities.Select(x => _roomServiceMapper.MapToRoomServiceModel(x)).ToList();

            for (int i = 0; i < roomServiceModels.Count; i++)
            {
                var roomService = roomServiceModels[i];

                var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(roomService.CreatedBy, roomService.LastUpdatedBy);

                roomService.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
                roomService.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);
            }

            return new ApiResponse().SetOk(roomServiceModels);
        }

        public async Task<ApiResponse> GetRoomSeviceSingle(Guid id)
        {
            var entity = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room service"));
            }

            var roomServiceModel = _roomServiceMapper.MapToRoomServiceModel(entity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(roomServiceModel.CreatedBy, roomServiceModel.LastUpdatedBy);

            roomServiceModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
            roomServiceModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

            return new ApiResponse().SetOk(roomServiceModel);
        }

        public async Task<ApiResponse> SoftDeleteRoomServiceBatch(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new ApiResponse().SetBadRequest("Request IDs must contain value");
            }

            var entities = await _unitOfWork.RoomServiceRepository.GetAllAsync(x => ids.Contains(x.Id));

            if (entities == null || entities.Count != ids.Count)
            {
                var foundedIds = entities?.Select(x => x.Id).ToList() ?? new List<Guid>();
                var notFoundEntities = ids.Except(foundedIds).ToList();

                return new ApiResponse().SetNotFound(notFoundEntities, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room service"));
            }

            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(ids);
        }

        public async Task<ApiResponse> UpdateRoomService(RoomServiceUpdateRequestModel model)
        {
            var entity = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room service"));
            }

            entity.RoomServiceName = model.RoomServiceName;
            entity.Unit = model.Unit;
            entity.Cost = model.Cost;
            entity.RoomServiceType = (RoomServiceTypeEnum)Enum.Parse(typeof(RoomServiceTypeEnum), model.RoomServiceType);
            entity.IsServiceIndicatorUsed = model.IsServiceIndicatorUsed;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }
    }
}
