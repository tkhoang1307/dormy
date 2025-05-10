using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly NotificationMapper _notificationMapper;

        public NotificationService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _notificationMapper = new NotificationMapper();
        }

        public async Task<List<NotificationModel>> GetNotifications()
        {
            var notificationEntities = new List<NotificationEntity>();
            bool isAdmin = _userContextService.UserRoles.Contains(Role.ADMIN);

            if (isAdmin)
            {
                notificationEntities = await _unitOfWork.NotificationRepository.GetAllAsync(x => !x.IsRead && !x.IsDeleted, x => x.Include(x => x.User), isNoTracking: true);
            }
            else
            {
                notificationEntities = await _unitOfWork.NotificationRepository.GetAllAsync(x => x.UserId == _userContextService.UserId && !x.IsRead && !x.IsDeleted, x => x.Include(x => x.User), isNoTracking: true);
            }

            List<NotificationModel> result = new List<NotificationModel>();

            result = notificationEntities.Select(_notificationMapper.MapToNotificationModel).OrderByDescending(x => x.Date).ToList();

            return result;
        }

        public async Task<ApiResponse> ReadNotification(Guid id)
        {
            var notifyEntity = await _unitOfWork.NotificationRepository.GetAsync(x => x.Id == id);

            if (notifyEntity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            notifyEntity.IsRead = true;
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> CreateNotification(NotificationRequestModel model)
        {
            var notificationEntity = new NotificationEntity()
            {
                Title = model.Title,
                Content = model.Content,
                Date = DateTime.UtcNow,
                IsRead = false,
                UserId = model.UserId,
                AdminId = model?.AdminId,
                NotificationType = NotificationTypeEnum.REGISTRATION_CREATION,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };

            await _unitOfWork.NotificationRepository.AddAsync(notificationEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated();
        }
    }
}
