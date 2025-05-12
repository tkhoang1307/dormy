using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse> GetNotifications();
        Task<ApiResponse> ReadNotification(Guid id);
        Task<ApiResponse> CreateNotification(NotificationRequestModel model);
        Task<ApiResponse> CreateAnnouncement(AnnouncementCreationRequestModel model);
    }
}
