using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class RoomTypeServiceService : IRoomTypeServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly RoomTypeServiceMapper _roomTypeServiceMapper;

        public RoomTypeServiceService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _roomTypeServiceMapper = new RoomTypeServiceMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> AddRoomTypeService(RoomTypeServiceCreationRequestModel model)
        {
            var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => model.RoomTypeIds.Contains(x.Id));
            if (roomTypes == null || roomTypes.Count != model.RoomTypeIds.Count)
            {
                return new ApiResponse().SetNotFound(message: ErrorMessages.SomeRoomTypesAreNotExisted);
            }

            var roomServices = await _unitOfWork.RoomServiceRepository.GetAllAsync(x => model.RoomServiceIds.Contains(x.Id));
            if (roomServices == null || roomServices.Count != model.RoomServiceIds.Count)
            {
                return new ApiResponse().SetNotFound(message: ErrorMessages.SomeServicesAreNotExisted);
            }

            var roomTypeServiceModel = new List<RoomTypeServiceRequestModel>();
            foreach(var roomTypeId in model.RoomTypeIds)
            {
                foreach(var roomServiceId in model.RoomServiceIds)
                {
                    roomTypeServiceModel.Add(new RoomTypeServiceRequestModel()
                    {
                        RoomTypeId = roomTypeId,
                        RoomServiceId = roomServiceId,
                    });
                }    
            }

            var roomTypeServiceEntity = roomTypeServiceModel.Select(x => _roomTypeServiceMapper.MapToRoomTypeServiceEntity(x)).ToList();

            for (var i = 0; i < roomTypeServiceEntity.Count; i++)
            {
                roomTypeServiceEntity[i].CreatedBy = _userContextService.UserId;
                roomTypeServiceEntity[i].LastUpdatedBy = _userContextService.UserId;
            }

            await _unitOfWork.RoomTypeServiceRepository.AddRangeAsync(roomTypeServiceEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated();
        }

        public async Task<ApiResponse> RemoveRoomTypeService(RoomTypeServiceDeletionRequestModel model)
        {
            var roomTypeService = await _unitOfWork.RoomTypeServiceRepository.
                                                    GetAllAsync(x => model.RoomServiceIds.Contains(x.RoomServiceId) &&
                                                                     model.RoomTypeIds.Contains(x.RoomTypeId));

            if (roomTypeService == null || roomTypeService.Count != model.RoomServiceIds.Count)
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.SomeTypeServicesAreNotExisted);
            }

            foreach(var entity in  roomTypeService)
            {
                await _unitOfWork.RoomTypeServiceRepository.DeleteByIdAsync(entity.Id);
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }
    }
}
