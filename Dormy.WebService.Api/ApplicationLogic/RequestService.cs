using Dormy.WebService.Api.Core.Constants;
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
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly RequestMapper _requestMapper;

        public RequestService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _requestMapper = new RequestMapper();
        }

        public async Task<ApiResponse> CreateRequest(RequestRequestModel model)
        {
            var requestEntity = _requestMapper.MapToRequestEntity(model);

            requestEntity.UserId = _userContextService.UserId;
            requestEntity.CreatedBy = _userContextService.UserId;
            requestEntity.LastUpdatedBy = _userContextService.UserId;

            if (model.RoomId != null)
            {
                var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == model.RoomId.Value, isNoTracking: true);
                if (roomEntity == null)
                {
                    return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
                }
                requestEntity.RoomId = model.RoomId.Value;
            }

            await _unitOfWork.RequestRepository.AddAsync(requestEntity);

            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == _userContextService.UserId, isNoTracking: true);

            NotificationEntity notificationEntity = new NotificationEntity()
            {
                Title = NotificationMessages.CreateRequestTitle,
                Content = string.Format(NotificationMessages.CreateRequestContent, $"{user?.FirstName} {user?.LastName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                Date = DateTime.UtcNow,
                IsRead = false,
                UserId = _userContextService.UserId,
                NotificationType = NotificationTypeEnum.REQUEST_CREATION,
            };

            await _unitOfWork.NotificationRepository.AddAsync(notificationEntity);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(requestEntity.Id);
        }

        public async Task<ApiResponse> GetRequests(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var requestEntities = new List<RequestEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => true,
                                                    x => x.Include(x => x.Approver)
                                                          .Include(x => x.User)
                                                          .Include(x => x.Room)
                                                            .ThenInclude(x => x.Building));
            }
            else
            {
                requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => x.UserId == userId,
                                                                                  x => x.Include(x => x.Approver)
                                                                                        .Include(x => x.User)
                                                                                        .Include(x => x.Room)
                                                                                            .ThenInclude(x => x.Building));
            }

            if (model.Ids.Count > 0)
            {
                requestEntities = requestEntities.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            var response = requestEntities.Select(x => _requestMapper.MapToRequestModel(x)).ToList();
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetSingleRequest(Guid id)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound(userId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "User"));
            }

            var requestEntity = new RequestEntity();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
            }
            else
            {
                requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
            }

            if (requestEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Request"));
            }

            if (_userContextService.UserRoles.Contains(Role.USER) && userId != requestEntity.UserId)
            {
                return new ApiResponse().SetForbidden(message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "request"));
            }

            var response = _requestMapper.MapToRequestModel(requestEntity);
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> UpdateRequest(RequestUpdationRequestModel model)
        {
            var requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == model.Id);
            if (requestEntity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Request"));
            }

            if (requestEntity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "request"));
            }

            if (requestEntity.Status != RequestStatusEnum.SUBMITTED)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.UpdateEntityConflict, "request"));
            }

            requestEntity.Description = model.Description;
            requestEntity.RequestType = model.RequestType;

            if (model.RoomId != null)
            {
                var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == model.RoomId.Value, isNoTracking: true);
                if (roomEntity == null)
                {
                    return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
                }
                requestEntity.RoomId = model.RoomId.Value;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(requestEntity.Id);
        }

        public async Task<ApiResponse> UpdateRequestStatus(Guid id, RequestStatusEnum status)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound(userId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "User"));
            }

            var isAdmin = _userContextService.UserRoles.Contains(Role.ADMIN);

            var requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, isNoTracking: false);

            if (requestEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Request"));
            }

            if (_userContextService.UserRoles.Contains(Role.USER) && _userContextService.UserId != requestEntity.UserId)
            {
                return new ApiResponse().SetForbidden(message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "overnight absence"));
            }

            var (isError, errorMessage) = RequestStatusChangeValidator.VerifyRequestStatusChangeValidator(requestEntity.Status, status);
            if (isError)
            {
                return new ApiResponse().SetConflict(id, message: string.Format(errorMessage, "Request"));
            }

            if (status == RequestStatusEnum.APPROVED || status == RequestStatusEnum.REJECTED)
            {
                requestEntity.ApproverId = userId;
            }

            requestEntity.Status = status;
            requestEntity.LastUpdatedBy = userId;
            requestEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            // Notification

            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == requestEntity.UserId, isNoTracking: true);
            var notifyActorName = $"{user?.FirstName} {user?.LastName}";

            if (isAdmin)
            {
                var adminAccount = await _unitOfWork.AdminRepository.GetAsync(x => x.Id == userId, isNoTracking: true);
                notifyActorName = $"{adminAccount?.FirstName} {adminAccount?.LastName}";
            }

            NotificationEntity notificationEntity = new NotificationEntity()
            {
                Title = string.Format(NotificationMessages.UpdateStatusRequestTitle, status.ToString()),
                Content = string.Format(NotificationMessages.UpdateStatusRequestContent, status.ToString(), notifyActorName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                Date = DateTime.UtcNow,
                IsRead = false,
                UserId = requestEntity.UserId,
                NotificationType = NotificationTypeEnum.REQUEST_STATUS_CHANGE,
                LastUpdatedBy = _userContextService.UserId,
                LastUpdatedDateUtc = DateTime.UtcNow
            };

            await _unitOfWork.NotificationRepository.AddAsync(notificationEntity);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(requestEntity.Id);
        }
    }
}
