using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class NotificationRequestModel
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public NotificationTypeEnum NotificationType { get; set; }
        public Guid UserId { get; set; }
        public Guid? AdminId { get; set; }
    }
}
