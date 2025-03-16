using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Org.BouncyCastle.Crypto;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private ParkingSpotMapper _parkingSpotMapper;

        public ParkingSpotService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _parkingSpotMapper = new ParkingSpotMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> AddNewParkingSpot(ParkingSpotRequestModel model)
        {
            var entity = _parkingSpotMapper.MapToParkingSpotEntity(model);

            entity.CreatedBy = _userContextService.UserId;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.ParkingSpotRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetDetailParkingSpot(Guid id)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            var parkingSpotModel = _parkingSpotMapper.MapToParkingSpotModel(entity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(parkingSpotModel.CreatedBy, parkingSpotModel.LastUpdatedBy);

            parkingSpotModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
            parkingSpotModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

            return new ApiResponse().SetOk(parkingSpotModel);
        }

        public async Task<ApiResponse> GetParkingSpotBatch(GetBatchRequestModel model)
        {
            var entities = new List<ParkingSpotEntity>();

            if (model.IsGetAll)
            {
                entities = await _unitOfWork.ParkingSpotRepository.GetAllAsync(x => true);
            }
            else
            {
                entities = await _unitOfWork.ParkingSpotRepository.GetAllAsync(x => model.Ids.Contains(x.Id));
            }

            var parkingSpotModels = new List<ParkingSpotResponseModel>();

            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER)
            {
                parkingSpotModels = entities.Where(x => x.IsDeleted == false)
                                            .Select(x => _parkingSpotMapper.MapToParkingSpotModel(x))
                                            .ToList();
            }   
            else
            {
                parkingSpotModels = entities.Select(x => _parkingSpotMapper.MapToParkingSpotModel(x)).ToList();

            }

            for (int i = 0; i < parkingSpotModels.Count; i++)
            {
                var parkingSpot = parkingSpotModels[i];

                var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(parkingSpot.CreatedBy, parkingSpot.LastUpdatedBy);

                parkingSpot.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
                parkingSpot.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);
            }

            return new ApiResponse().SetOk(parkingSpotModels);
        }

        public async Task<ApiResponse> UpdateParkingSpot(ParkingSpotUpdationRequestModel model)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            entity.CapacitySpots = model.CapacitySpots;
            entity.ParkingSpotName = model.ParkingSpotName;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        public async Task<ApiResponse> SoftDeleteParkingSpot(Guid id)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == id);

            if (entity == null )
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            entity.IsDeleted = true;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(id);
        }

        public async Task<ApiResponse> UpdateStatusParkingSpot(ParkingSpotUpdateStatusRequestModel model)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            entity.Status = (ParkingSpotStatusEnum)Enum.Parse(typeof(GenderEnum), model.Status);
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }
    }
}
