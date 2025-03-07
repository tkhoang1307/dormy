﻿using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ParkingRequestService : IParkingRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public ParkingRequestService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> CreateParkingRequest(ParkingRequestModel model)
        {
            await VerirfyParkingRequest(model.ParkingSpotId, model.VehicleId);

            var parkingRequest = new ParkingRequestEntity
            {
                Id = Guid.NewGuid(),
                CreatedBy = _userContextService.UserId,
                ParkingSpotId = model.ParkingSpotId,
                Status = Models.Enums.RequestStatusEnum.SUBMITTED,
                UserId = _userContextService.UserId,
                VehicleId = model.VehicleId,
                Description = model.Description,
            };

            await _unitOfWork.ParkingRequestRepository.AddAsync(parkingRequest);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(parkingRequest.Id);
        }

        public async Task<ApiResponse> GetParkingRequestBatch(List<Guid> ids, bool isGetAll = false)
        {
            var parkingRequests = await _unitOfWork.ParkingRequestRepository.GetAllAsync(x => isGetAll || ids.Contains(x.Id),
                x => x
                .Include(x => x.Vehicle)
                .Include(x => x.Approver)
                .Include(x => x.ParkingSpot));

            var result = parkingRequests.Select(x => new ParkingRequestResponseModel
            {
                Id = x.Id,
                Description = x.Description,
                Status = x.Status,
                UserId = x.UserId,
                VehicleId = x.VehicleId,
                ParkingSpotId = x.ParkingSpotId,
                ApproverId = x.ApproverId,
                ApproverUserFullName = x.Approver.LastName + " " + x.Approver.FirstName,
                ApproverUserName = x.Approver.UserName,
                LicensePlate = x.Vehicle.LicensePlate,
                VehicleType = x.Vehicle.VehicleType,
                UserName = x.User.UserName,
                ParkingSpotName = x.ParkingSpot.ParkingSpotName,
                ParkingSpotStatus = x.ParkingSpot.Status,
                UserFullName = x.User.LastName + " " + x.User.FirstName
            }).ToList();

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetSingleParkingRequest(Guid id)
        {
            var parkingRequest = await _unitOfWork.ParkingRequestRepository.GetAsync(x => x.Id == id,
                x => x
                .Include(x => x.Vehicle)
                .Include(x => x.Approver)
                .Include(x => x.ParkingSpot));

            if (parkingRequest == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking request"));
            }

            var result = new ParkingRequestResponseModel
            {
                Id = parkingRequest.Id,
                Description = parkingRequest.Description,
                Status = parkingRequest.Status,
                UserId = parkingRequest.UserId,
                VehicleId = parkingRequest.VehicleId,
                ParkingSpotId = parkingRequest.ParkingSpotId,
                ApproverId = parkingRequest.ApproverId,
                ApproverUserFullName = parkingRequest.Approver.LastName + " " + parkingRequest.Approver.FirstName,
                ApproverUserName = parkingRequest.Approver.UserName,
                LicensePlate = parkingRequest.Vehicle.LicensePlate,
                VehicleType = parkingRequest.Vehicle.VehicleType,
                UserName = parkingRequest.User.UserName,
                ParkingSpotName = parkingRequest.ParkingSpot.ParkingSpotName,
                ParkingSpotStatus = parkingRequest.ParkingSpot.Status,
                UserFullName = parkingRequest.User.LastName + " " + parkingRequest.User.FirstName
            };

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> UpdateParkingRequestStatus(ParkingRequestStatusModel model)
        {
            var parkingRequest = await _unitOfWork.ParkingRequestRepository.GetAsync(x => x.Id == model.Id);

            if (parkingRequest == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking request"));
            }

            var (isError, errorMessage) = RequestStatusChangeValidator.VerifyRequestStatusChangeValidator(parkingRequest.Status, model.Status);
            if (isError)
            {
                return new ApiResponse().SetConflict(parkingRequest.Id,
                                                     message: string.Format(errorMessage, "parking request"));
            }

            parkingRequest.Status = model.Status;
            parkingRequest.LastUpdatedBy = _userContextService.UserId;
            parkingRequest.LastUpdatedDateUtc = DateTime.Now;

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.Status == Models.Enums.RequestStatusEnum.APPROVED || model.Status == Models.Enums.RequestStatusEnum.REJECTED)
                {
                    parkingRequest.ApproverId = _userContextService.UserId;
                }

                if (model.Status == Models.Enums.RequestStatusEnum.APPROVED)
                {
                    var vehicle = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == parkingRequest.VehicleId);
                    vehicle.ParkingSpotId = parkingRequest.ParkingSpotId;
                }
            };

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(parkingRequest.Id);
        }

        public async Task<ApiResponse> UpdateParkingRequest(UpdateParkingRequestModel model)
        {
            var parkingRequest = await _unitOfWork.ParkingRequestRepository.GetAsync(x => x.Id == model.Id);

            if (parkingRequest == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking request"));
            }

            if (parkingRequest.Status != Models.Enums.RequestStatusEnum.SUBMITTED)
            {
                return new ApiResponse().SetConflict(parkingRequest.Id,
                                                     message: string.Format(ErrorMessages.UpdateEntityConflict, "parking request"));
            }    

            await VerirfyParkingRequest(model.ParkingSpotId, parkingRequest.VehicleId);

            parkingRequest.Description = model.Description;
            parkingRequest.ParkingSpotId = model.ParkingSpotId;
            parkingRequest.LastUpdatedBy = _userContextService.UserId;
            parkingRequest.LastUpdatedDateUtc = DateTime.Now;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(parkingRequest.Id);
        }

        public async Task<ApiResponse> SoftDeleteParkingRequest(Guid id)
        {
            var parkingRequest = await _unitOfWork.ParkingRequestRepository.GetAsync(x => x.Id == id);

            if (parkingRequest == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking request"));
            }

            parkingRequest.IsDeleted = true;
            parkingRequest.LastUpdatedBy = _userContextService.UserId;
            parkingRequest.LastUpdatedDateUtc = DateTime.Now;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(parkingRequest.Id);
        }

        private async Task VerirfyParkingRequest(Guid parkingSpotId, Guid vehicleId)
        {
            var parkingSpot = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == parkingSpotId);
            var vehicle = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == vehicleId);

            if (parkingSpot is null || vehicle is null)
            {
                var entityName = parkingSpot is null ? "Parking Spot" : "Vehicle";
                var entityId = parkingSpot is null ? parkingSpotId : vehicleId;
                throw new EntityNotFoundException($"{entityName} with Id: {entityId} Not Found.");
            }
        }
    }
}
