using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;

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
            var entity = _roomTypeMapper.MapToRoomTypeEnity(model);
            entity.CreatedBy = _userContextService.UserId;

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
                entities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => true);
            }
            else
            {
                entities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => x.isDeleted == false);
            }

            var response = entities.Select(entity => _roomTypeMapper.MapToRoomTypeResponseModel(entity)).ToList();

            return new ApiResponse().SetOk(response);
        }
    }
}
