using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class VehicleHistoryService : IVehicleHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public VehicleHistoryService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> CreateVehicleHistory(VehicleHistoryRequestModel model)
        {
            var vehicleEntity = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == model.VehicleId, isNoTracking: true);
            if (vehicleEntity == null)
            {
                return new ApiResponse().SetNotFound("Vehicle not found");
            }

            var parkingSpotEntity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.ParkingSpotId, isNoTracking: false);
            if (parkingSpotEntity == null)
            {
                return new ApiResponse().SetNotFound("Parking spot not found");
            }

            if (parkingSpotEntity.Status == Models.Enums.ParkingSpotStatusEnum.UNDER_MAINTENANCE)
            {
                return new ApiResponse().SetBadRequest("Parking spot is under maintenance");
            }

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                if (model.IsIn)
                {
                    if (parkingSpotEntity.CurrentQuantity >= parkingSpotEntity.CapacitySpots)
                    {
                        return new ApiResponse().SetBadRequest("Parking spot is full");
                    }
                    parkingSpotEntity.CurrentQuantity++;
                    await _unitOfWork.SaveChangeAsync();
                }
                else
                {
                    parkingSpotEntity.CurrentQuantity = parkingSpotEntity.CurrentQuantity == 0 ? 0 : parkingSpotEntity.CurrentQuantity - 1;
                    await _unitOfWork.SaveChangeAsync();
                }

                var vehicleHistoryEntity = new VehicleHistoryEntity
                {
                    VehicleId = model.VehicleId,
                    ParkingSpotId = model.ParkingSpotId,
                    Action = model.IsIn ? Models.Enums.VehicleActionEnum.IN : Models.Enums.VehicleActionEnum.OUT,
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = _userContextService.UserId
                };

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
                return new ApiResponse().SetNotFound("Vehicle history not found");
            }

            var result = new VehicleHisotryResponseModel
            {
                Id = vehicleHistoryEntity.Id,
                VehicleId = vehicleHistoryEntity.VehicleId,
                ParkingSpotId = vehicleHistoryEntity.ParkingSpotId,
                Action = vehicleHistoryEntity.Action.ToString(),
                CurrentQuantity = vehicleHistoryEntity.ParkingSpot.CurrentQuantity,
                CapacitySpots = vehicleHistoryEntity.ParkingSpot.CapacitySpots,
                LicensePlate = vehicleHistoryEntity.Vehicle.LicensePlate,
                VehicleType = vehicleHistoryEntity.Vehicle.VehicleType,
                ParkingSpotName = vehicleHistoryEntity.ParkingSpot.ParkingSpotName,
                Status = vehicleHistoryEntity.ParkingSpot.Status.ToString(),
                IsDeleted = vehicleHistoryEntity.IsDeleted,
                CreatedAt = vehicleHistoryEntity.CreatedDateUtc,
                LastUpdatedAt = vehicleHistoryEntity.LastUpdatedDateUtc.HasValue ? vehicleHistoryEntity.LastUpdatedDateUtc.Value : vehicleHistoryEntity.CreatedDateUtc,
            };

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

            var result = vehicleHistoryEntities.Select(x => new VehicleHisotryResponseModel
            {
                Id = x.Id,
                VehicleId = x.VehicleId,
                ParkingSpotId = x.ParkingSpotId,
                Action = x.Action.ToString(),
                CurrentQuantity = x.ParkingSpot.CurrentQuantity,
                CapacitySpots = x.ParkingSpot.CapacitySpots,
                LicensePlate = x.Vehicle.LicensePlate,
                VehicleType = x.Vehicle.VehicleType,
                ParkingSpotName = x.ParkingSpot.ParkingSpotName,
                Status = x.ParkingSpot.Status.ToString(),
                IsDeleted = x.IsDeleted,
                CreatedAt = x.CreatedDateUtc,
                LastUpdatedAt = x.LastUpdatedDateUtc.HasValue ? x.LastUpdatedDateUtc.Value : x.CreatedDateUtc,
            }).ToList();

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> SoftDeleteVehicleHistory(Guid id)
        {
            var vehicleHistoryEntity = await _unitOfWork.VehicleHistoryRepository.GetAsync(x => x.Id == id);
            if (vehicleHistoryEntity == null)
            {
                return new ApiResponse().SetNotFound("Vehicle history not found");
            }

            vehicleHistoryEntity.IsDeleted = true;
            vehicleHistoryEntity.LastUpdatedDateUtc = DateTime.UtcNow;
            vehicleHistoryEntity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetAccepted();
        }
    }
}
