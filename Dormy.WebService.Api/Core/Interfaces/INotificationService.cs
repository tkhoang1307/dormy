using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationModel>> GetNotifications();
        Task<ApiResponse> ReadNotification(Guid id);
    }
}
