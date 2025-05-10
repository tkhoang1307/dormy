using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Presentation.Validations;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ParkingRequestService : IParkingRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ParkingRequestMapper _parkingRequestMapper;
        private readonly INotificationService _notificationService;

        public ParkingRequestService(IUnitOfWork unitOfWork, IUserContextService userContextService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _parkingRequestMapper = new ParkingRequestMapper();
            _notificationService = notificationService;
        }

        public async Task<ApiResponse> CreateParkingRequest(ParkingRequestModel model)
        {
            var parkingSpotEntity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.ParkingSpotId);
            if (parkingSpotEntity == null)
            {
                return new ApiResponse().SetNotFound(model.ParkingSpotId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }
            if (parkingSpotEntity.CurrentQuantity == parkingSpotEntity.CapacitySpots)
            {
                return new ApiResponse().SetPreconditionFailed(model.ParkingSpotId, message: string.Format(ErrorMessages.ParkingRequestIsFull));
            }

            var vehicleEntity = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == model.VehicleId);
            if (vehicleEntity == null)
            {
                return new ApiResponse().SetNotFound(model.VehicleId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Vehicle"));
            }

            var parkingRequestEntity = _parkingRequestMapper.MapToParkingRequestEntity(model);
            parkingRequestEntity.UserId = _userContextService.UserId;
            parkingRequestEntity.CreatedBy = _userContextService.UserId;
            parkingRequestEntity.LastUpdatedBy = _userContextService.UserId;

            parkingSpotEntity.CurrentQuantity += 1;
            if (parkingSpotEntity.CurrentQuantity == parkingSpotEntity.CapacitySpots)
            {
                parkingSpotEntity.Status = ParkingSpotStatusEnum.FULL;
            }

            await _unitOfWork.ParkingRequestRepository.AddAsync(parkingRequestEntity);

            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == _userContextService.UserId, isNoTracking: false);

            await _notificationService.CreateNotification(new NotificationRequestModel()
            {
                Title = NotificationMessages.CreateParkingRequestTitle,
                Content = string.Format(NotificationMessages.CreateParkingRequestContent, $"{user.FirstName} {user.LastName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                UserId = _userContextService.UserId,
                NotificationType = NotificationTypeEnum.PARKING_REQUEST_CREATION,
            });

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(parkingRequestEntity.Id);
        }

        public async Task<ApiResponse> GetParkingRequestBatch(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var parkingRequests = new List<ParkingRequestEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                parkingRequests = await _unitOfWork.ParkingRequestRepository
                    .GetAllAsync(x => true, x => x
                    .Include(x => x.Vehicle)
                    .Include(x => x.Approver)
                    .Include(x => x.ParkingSpot)
                    .Include(x => x.User));
            }
            else
            {
                parkingRequests = await _unitOfWork.ParkingRequestRepository
                    .GetAllAsync(x => x.UserId.Equals(userId), x => x
                    .Include(x => x.Vehicle)
                    .Include(x => x.Approver)
                    .Include(x => x.ParkingSpot)
                    .Include(x => x.User));
            }

            if (model.Ids.Count > 0)
            {
                parkingRequests = parkingRequests.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            var result = parkingRequests.Select(x => _parkingRequestMapper.MapToParkingRequestResponseModel(x)).ToList();

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetSingleParkingRequest(Guid id)
        {
            var parkingRequest = await _unitOfWork.ParkingRequestRepository.GetAsync(x => x.Id == id,
                x => x
                .Include(x => x.Vehicle)
                .Include(x => x.Approver)
                .Include(x => x.ParkingSpot)
                .Include(x => x.User));

            if (parkingRequest == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking request"));
            }

            var result = _parkingRequestMapper.MapToParkingRequestResponseModel(parkingRequest);

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> UpdateParkingRequestStatus(ParkingRequestStatusModel model)
        {
            bool isAdmin = _userContextService.UserRoles.Contains(Role.ADMIN);
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

            if (model.Status == RequestStatusEnum.REJECTED || model.Status == RequestStatusEnum.CANCELLED)
            {
                var parkingSpotEntity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == parkingRequest.ParkingSpotId);

                if (parkingSpotEntity == null)
                {
                    return new ApiResponse().SetNotFound($"Parking Spot Not Found ${parkingRequest.ParkingSpotId}");
                }

                if (parkingSpotEntity.CurrentQuantity == parkingSpotEntity.CapacitySpots)
                {
                    parkingSpotEntity.Status = ParkingSpotStatusEnum.AVAILABLE;
                }
                parkingSpotEntity.CurrentQuantity = parkingSpotEntity.CurrentQuantity - 1;
            }

            parkingRequest.Status = model.Status;
            parkingRequest.LastUpdatedBy = _userContextService.UserId;
            parkingRequest.LastUpdatedDateUtc = DateTime.Now;

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.Status == RequestStatusEnum.APPROVED || model.Status == RequestStatusEnum.REJECTED)
                {
                    parkingRequest.ApproverId = _userContextService.UserId;
                }

                if (model.Status == RequestStatusEnum.APPROVED)
                {
                    var vehicle = await _unitOfWork.VehicleRepository.GetAsync(x => x.Id == parkingRequest.VehicleId);
                    if (vehicle != null)
                    {
                        vehicle.ParkingSpotId = parkingRequest.ParkingSpotId;
                    }
                }
            };

            // Notification

            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == parkingRequest.UserId, isNoTracking: true);
            var notifyActorName = $"{user?.FirstName} {user?.LastName}";

            if (isAdmin)
            {
                var adminAccount = await _unitOfWork.AdminRepository.GetAsync(x => x.Id == _userContextService.UserId);
                notifyActorName = $"{adminAccount?.FirstName} {adminAccount?.LastName}";
            }

            await _notificationService.CreateNotification(new NotificationRequestModel()
            {
                Title = string.Format(NotificationMessages.UpdateParkingRequestStatusTitle, model.Status.ToString()),
                Content = string.Format(NotificationMessages.UpdateParkingRequestStatusContent, model.Status.ToString(), notifyActorName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                UserId = parkingRequest.UserId,
                NotificationType = NotificationTypeEnum.PARKING_REQUEST_STATUS_CHANGE,
            });

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

            if (parkingRequest.Status != RequestStatusEnum.SUBMITTED)
            {
                return new ApiResponse().SetConflict(parkingRequest.Id,
                                                     message: string.Format(ErrorMessages.UpdateEntityConflict, "parking request"));
            }

            var newParkingSpotEntity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.ParkingSpotId);
            if (newParkingSpotEntity == null)
            {
                return new ApiResponse().SetNotFound(model.ParkingSpotId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }
            if (newParkingSpotEntity.CurrentQuantity == newParkingSpotEntity.CapacitySpots)
            {
                return new ApiResponse().SetPreconditionFailed(model.ParkingSpotId, message: string.Format(ErrorMessages.ParkingRequestIsFull));
            }

            // Update slot for new parking spot
            newParkingSpotEntity.CurrentQuantity = newParkingSpotEntity.CurrentQuantity + 1;
            if (newParkingSpotEntity.CurrentQuantity == newParkingSpotEntity.CapacitySpots)
            {
                newParkingSpotEntity.Status = ParkingSpotStatusEnum.FULL;
            }

            //Update slot for current parking spot
            var currentParkingSpotEntity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == parkingRequest.ParkingSpotId);
            if (currentParkingSpotEntity.CurrentQuantity == currentParkingSpotEntity.CapacitySpots)
            {
                currentParkingSpotEntity.Status = ParkingSpotStatusEnum.AVAILABLE;
            }
            currentParkingSpotEntity.CurrentQuantity = currentParkingSpotEntity.CurrentQuantity - 1;

            parkingRequest.Description = model.Description;
            parkingRequest.ParkingSpotId = model.ParkingSpotId;
            parkingRequest.LastUpdatedBy = _userContextService.UserId;
            parkingRequest.LastUpdatedDateUtc = DateTime.Now;

            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == _userContextService.UserId, isNoTracking: false);

            await _notificationService.CreateNotification(new NotificationRequestModel()
            {
                Title = NotificationMessages.UpdateParkingRequestTitle,
                Content = string.Format(NotificationMessages.UpdateParkingRequestContent, $"{user.FirstName} {user.LastName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                UserId = _userContextService.UserId,
                NotificationType = NotificationTypeEnum.PARKING_REQUEST_UPDATION,
            });

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
    }
}
