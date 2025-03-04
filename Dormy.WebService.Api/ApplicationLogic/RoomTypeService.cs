using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly RoomTypeMapper _roomTypeMapper;

        public RoomTypeService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _roomTypeMapper = new RoomTypeMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> CreateRoomType(RoomTypeRequestModel model)
        {
            var roomServices = await _unitOfWork.RoomServiceRepository.GetAllAsync(x => model.RoomServiceIds.Contains(x.Id));

            if (roomServices == null || roomServices.Count != model.RoomServiceIds.Count)
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.SomeServicesAreNotExisted);
            }

            var entity = _roomTypeMapper.MapToRoomTypeEnity(model);
            entity.CreatedBy = _userContextService.UserId;
            entity.LastUpdatedBy = _userContextService.UserId;
            for (var i = 0; i < entity.RoomTypeServices.Count; i++)
            {
                entity.RoomTypeServices[i].CreatedBy = _userContextService.UserId;
                entity.RoomTypeServices[i].LastUpdatedBy = _userContextService.UserId;
            }    

            await _unitOfWork.RoomTypeRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetRoomTypes()
        {
            var entities = new List<RoomTypeEntity>();
            var isAdmin = _userContextService.UserRoles.Any(x => x.Trim().Contains(Role.ADMIN.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (isAdmin)
            {
                entities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => true, x => x.Include(x => x.RoomTypeServices)
                                                               .ThenInclude(x => x.RoomService));
            }
            else
            {
                entities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => x.IsDeleted == false, x => x.Include(x => x.RoomTypeServices)
                                                               .ThenInclude(x => x.RoomService));
            }

            var response = entities.Select(entity => _roomTypeMapper.MapToRoomTypeResponseModel(entity)).ToList();

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetRoomTypeById(Guid id)
        {
            RoomTypeEntity? entity = null;
            var isAdmin = _userContextService.UserRoles.Any(x => x.Trim().Contains(Role.ADMIN.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (isAdmin)
            {
                entity = await _unitOfWork.RoomTypeRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.RoomTypeServices)
                                                             .ThenInclude(x => x.RoomService));
            }
            else
            {
                entity = await _unitOfWork.RoomTypeRepository.GetAsync(x => x.Id == id && x.IsDeleted == false, x => x.Include(x => x.RoomTypeServices)
                                                             .ThenInclude(x => x.RoomService));
            }

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room type"));
            }

            var response = _roomTypeMapper.MapToRoomTypeResponseModel(entity);

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> SoftDeleteRoomType(Guid id)
        {
            var entity = await _unitOfWork.RoomTypeRepository.GetAsync(x => x.Id.Equals(id), include: x => x.Include(x => x.Rooms));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room type"));
            }

            if (entity.Rooms != null && entity.Rooms.Any(r => r.TotalUsedBed > 0))
            {
                return new ApiResponse().SetBadRequest(id, ErrorMessages.RoomIsOccupiedErrorMessage);
            }

            entity.IsDeleted = true;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(id);
        }

        public async Task<ApiResponse> UpdateRoomType(RoomTypeUpdateRequestModel model)
        {
            var roomServices = await _unitOfWork.RoomServiceRepository.GetAllAsync(x => model.RoomServiceIds.Contains(x.Id));

            if (roomServices == null || roomServices.Count != model.RoomServiceIds.Count)
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.SomeServicesAreNotExisted);
            }

            var entity = await _unitOfWork.RoomTypeRepository.GetAsync(x => x.Id.Equals(model.Id), x => x.Include(x => x.Rooms));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room type"));
            }

            //if (model.Capacity < entity.Rooms?.Count)
            //{
            //    return new ApiResponse().SetBadRequest(model.Id, ErrorMessages.RoomCapacityIsSmallerThanCurrentErrorMessage);
            //}

            entity.RoomTypeName = model.RoomTypeName;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.Capacity = model.Capacity;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            // update roomTypeServices
            var roomTypeServices = await _unitOfWork.RoomTypeServiceRepository.GetAllAsync(x => x.RoomTypeId == model.Id);
            var roomTypeServiceIds = roomTypeServices.Select(x => x.Id).ToList();
            foreach (var roomTypeServiceId in roomTypeServiceIds)
            {
                await _unitOfWork.RoomTypeServiceRepository.DeleteByIdAsync(roomTypeServiceId);
            }

            await _unitOfWork.RoomTypeServiceRepository
                .AddRangeAsync(model.RoomServiceIds
                .Select(rs => new RoomTypeServiceEntity()
                {
                    Id = Guid.NewGuid(),
                    RoomServiceId = rs,
                    RoomTypeId = entity.Id,
                    CreatedBy = _userContextService.UserId,
                    LastUpdatedBy = _userContextService.UserId,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastUpdatedDateUtc = DateTime.UtcNow,
                    IsDeleted = false,
                }).ToList());

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }
    }
}
