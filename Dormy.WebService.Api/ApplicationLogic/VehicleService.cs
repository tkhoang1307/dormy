﻿using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private VehicleMapper _vehicleMapper;

        public VehicleService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _vehicleMapper = new VehicleMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> AddNewVehicle(VehicleRequestModel model)
        {
            var entity = _vehicleMapper.MapToVehicleEntity(model);

            entity.UserId = model?.UserId ?? _userContextService.UserId;
            entity.CreatedBy = model?.UserId ?? _userContextService.UserId;
            entity.LastUpdatedBy = model?.UserId ?? _userContextService.UserId;

            await _unitOfWork.VehicleRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetDetailVehicleById(Guid id)
        {
            var entity = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == id, include: vehicle => vehicle.Include(v => v.User));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Vehicle"));
            }

            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER && entity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(message: ErrorMessages.AccountDoesNotHavePermission);
            }

            var vehicleModel = _vehicleMapper.MapToVehicleResponseModel(entity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(vehicleModel.CreatedBy, vehicleModel.LastUpdatedBy);

            vehicleModel.CreatedByCreator = UserHelper.ConvertUserIdToUserFullname(createdUser);
            vehicleModel.LastUpdatedByUpdater = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);
            vehicleModel.UserFullname = entity.User.FirstName + " " + entity.User.LastName;

            return new ApiResponse().SetOk(vehicleModel);
        }

        public async Task<ApiResponse> GetVehicleBatch(GetBatchVehicleRequestModel model)
        {
            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER)
            {
                return await GetAllVehiclesOfUser();
            }

            var entities = new List<VehicleEntity>();

            entities = await _unitOfWork.VehicleRepository.GetAllAsync(x => true, include: vehicle => vehicle.Include(v => v.User));

            if (model.UserId != null)
            {
                entities = entities.Where(x => x.UserId == model.UserId).ToList();
            }

            if (model.Ids.Count > 0)
            {
                entities = entities.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            var vehicleModels = new List<VehicleResponseModel>();

            for (int i = 0; i < entities.Count; i++)
            {
                var vehicleEntity = entities[i];
                var vehicle = _vehicleMapper.MapToVehicleResponseModel(vehicleEntity);

                var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(vehicle.CreatedBy, vehicle.LastUpdatedBy);

                vehicle.CreatedByCreator = UserHelper.ConvertUserIdToUserFullname(createdUser);
                vehicle.LastUpdatedByUpdater = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);
                vehicle.UserFullname = vehicleEntity.User.FirstName + " " + vehicleEntity.User.LastName;
                if (vehicleEntity.ParkingSpotId != null && vehicleEntity.ParkingSpotId != Guid.Empty)
                {
                    vehicle.ParkingSpotId = vehicleEntity.ParkingSpotId;
                    var parkingSpot = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == vehicleEntity.ParkingSpotId);
                    if (parkingSpot != null)
                    {
                        vehicle.ParkingSpotName = parkingSpot.ParkingSpotName;
                    }
                }
                vehicleModels.Add(vehicle);
            }

            return new ApiResponse().SetOk(vehicleModels);
        }

        public async Task<ApiResponse> GetAllVehiclesOfUser()
        {
            var entities = await _unitOfWork.VehicleRepository.GetAllAsync(x => x.UserId == _userContextService.UserId, include: vehicle => vehicle.Include(v => v.User));

            var vehicleModels = new List<VehicleResponseModel>();

            for (int i = 0; i < entities.Count; i++)
            {
                var vehicleEntity = entities[i];
                var vehicle = _vehicleMapper.MapToVehicleResponseModel(vehicleEntity);

                var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(vehicle.CreatedBy, vehicle.LastUpdatedBy);

                vehicle.CreatedByCreator = UserHelper.ConvertUserIdToUserFullname(createdUser);
                vehicle.LastUpdatedByUpdater = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);
                vehicle.UserFullname = vehicleEntity.User.FirstName + " " + vehicleEntity.User.LastName;

                if (vehicleEntity.ParkingSpotId != null && vehicleEntity.ParkingSpotId != Guid.Empty)
                {
                    vehicle.ParkingSpotId = vehicleEntity.ParkingSpotId;
                    var parkingSpot = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == vehicleEntity.ParkingSpotId);
                    if (parkingSpot != null)
                    {
                        vehicle.ParkingSpotName = parkingSpot.ParkingSpotName;
                    }
                }
                vehicleModels.Add(vehicle);
            }

            return new ApiResponse().SetOk(vehicleModels);
        }

        public async Task<ApiResponse> UpdateVehicle(VehicleUpdationRequestModel model)
        {
            var entity = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Vehicle"));
            }

            if (entity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(message: ErrorMessages.AccountDoesNotHavePermission);
            }

            entity.LicensePlate = model.LicensePlate;
            entity.VehicleType = model.VehicleType;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        public async Task<ApiResponse> SoftDeleteVehicle(Guid id)
        {
            var entity = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == id);
            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Vehicle"));
            }

            entity.IsDeleted = true;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(id);
        }
    }
}
