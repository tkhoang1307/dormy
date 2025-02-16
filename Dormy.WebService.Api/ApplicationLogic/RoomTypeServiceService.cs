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
            var roomType = await _unitOfWork.RoomTypeRepository.GetAsync(x => x.Id == model.RoomTypeId);

            if (roomType == null)
            {
                return new ApiResponse().SetNotFound(model.RoomTypeId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room type"));
            }

            var roomServices = await _unitOfWork.RoomServiceRepository.GetAllAsync(x => model.RoomServiceIds.Contains(x.Id));

            if (roomServices == null || roomServices.Count != model.RoomServiceIds.Count)
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.SomeServicesAreNotExisted);
            }

            var roomTypeServiceModel = new List<RoomTypeServiceRequestModel>();
            foreach (var roomService in model.RoomServiceIds)
            {
                roomTypeServiceModel.Add(new RoomTypeServiceRequestModel()
                {
                    RoomTypeId = model.RoomTypeId,
                    RoomServiceId = roomService,
                });
            }

            var roomTypeServiceEntity = roomTypeServiceModel.Select(x => _roomTypeServiceMapper.MapToRoomTypeServiceEntity(x)).ToList();

            for (var i = 0; i < roomTypeServiceEntity.Count; i++)
            {
                roomTypeServiceEntity[i].CreatedBy = _userContextService.UserId;
                roomTypeServiceEntity[i].LastUpdatedBy = _userContextService.UserId;
            }

            await _unitOfWork.RoomTypeServiceRepository.AddRangeAsync(roomTypeServiceEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(model.RoomTypeId);
        }

        public async Task<ApiResponse> RemoveRoomTypeService(RoomTypeServiceDeletionRequestModel model)
        {
            var roomTypeService = await _unitOfWork.RoomTypeServiceRepository.
                                                    GetAllAsync(x => model.RoomServiceIds.Contains(x.RoomServiceId) && x.RoomTypeId == model.RoomTypeId);

            if (roomTypeService == null || roomTypeService.Count != model.RoomServiceIds.Count)
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.SomeTypeServicesAreNotExisted);
            }

            foreach(var entity in  roomTypeService)
            {
                await _unitOfWork.RoomTypeServiceRepository.DeleteByIdAsync(entity.Id);
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(model.RoomTypeId);
        }
    }
}
