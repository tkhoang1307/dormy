using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class VehicleHistoryService : IVehicleHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly VehicleHistoryMapper _vehicleHistoryMapper;

        public VehicleHistoryService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _vehicleHistoryMapper = new VehicleHistoryMapper();
        }

        public async Task<ApiResponse> CreateVehicleHistory(VehicleHistoryRequestModel model)
        {
            var vehicleEntity = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == model.VehicleId, isNoTracking: true);
            if (vehicleEntity == null)
            {
                return new ApiResponse().SetNotFound(model.VehicleId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Vehicle"));
            }

            var parkingSpotEntity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.ParkingSpotId, isNoTracking: false);
            if (parkingSpotEntity == null)
            {
                return new ApiResponse().SetNotFound(model.ParkingSpotId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            if (parkingSpotEntity.Status == Models.Enums.ParkingSpotStatusEnum.UNDER_MAINTENANCE)
            {
                return new ApiResponse().SetBadRequest(message: ErrorMessages.ParkingSpotIsUnderMaintenance);
            }

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                //if (model.IsIn)
                //{
                //    if (parkingSpotEntity.CurrentQuantity >= parkingSpotEntity.CapacitySpots)
                //    {
                //        return new ApiResponse().SetBadRequest(message: ErrorMessages.ParkingSpotIsFull);
                //    }
                //    parkingSpotEntity.CurrentQuantity++;
                //    await _unitOfWork.SaveChangeAsync();
                //}
                //else
                //{
                //    parkingSpotEntity.CurrentQuantity = parkingSpotEntity.CurrentQuantity == 0 ? 0 : parkingSpotEntity.CurrentQuantity - 1;
                //    await _unitOfWork.SaveChangeAsync();
                //}

                var vehicleHistoryEntity = _vehicleHistoryMapper.MapToVehicleHistoryEntity(model);

                vehicleHistoryEntity.CreatedBy = _userContextService.UserId;
                vehicleHistoryEntity.LastUpdatedBy = _userContextService.UserId;

                await _unitOfWork.VehicleHistoryRepository.AddAsync(vehicleHistoryEntity);
                await _unitOfWork.SaveChangeAsync();
                scope.Complete();
            }

            return new ApiResponse().SetCreated();
        }

        public async Task<ApiResponse> GetSingleVehicleHistory(Guid id)
        {
            var vehicleHistoryEntity = await _unitOfWork.VehicleHistoryRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Vehicle).Include(x => x.ParkingSpot), isNoTracking: true);
            if (vehicleHistoryEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Vehicle history"));
            }

            var result = _vehicleHistoryMapper.MapToVehicleHistoryResponseModel(vehicleHistoryEntity);

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetVehicleHistories(GetBatchRequestModel model)
        {
            var vehicleHistoryEntities = new List<VehicleHistoryEntity>();
            if (model.IsGetAll)
            {
                vehicleHistoryEntities = await _unitOfWork.VehicleHistoryRepository.GetAllAsync(x => true, x => x.Include(x => x.Vehicle).Include(x => x.ParkingSpot), isNoTracking: true);
            }
            else
            {
                vehicleHistoryEntities = await _unitOfWork.VehicleHistoryRepository.GetAllAsync(x => model.Ids.Contains(x.Id), x => x.Include(x => x.Vehicle).Include(x => x.ParkingSpot), isNoTracking: true);
            }

            var result = vehicleHistoryEntities.Select(x => _vehicleHistoryMapper.MapToVehicleHistoryResponseModel(x)).ToList();

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> SoftDeleteVehicleHistory(Guid id)
        {
            var vehicleHistoryEntity = await _unitOfWork.VehicleHistoryRepository.GetAsync(x => x.Id == id);
            if (vehicleHistoryEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Vehicle history"));
            }

            vehicleHistoryEntity.IsDeleted = true;
            vehicleHistoryEntity.LastUpdatedDateUtc = DateTime.UtcNow;
            vehicleHistoryEntity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetAccepted();
        }
    }
}
