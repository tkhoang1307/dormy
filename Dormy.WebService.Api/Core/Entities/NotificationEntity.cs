using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class NotificationEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Content { get; set; } = string.Empty;

        public DateTime Date {  get; set; } = DateTime.Now;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public NotificationTypeEnum NotificationType { get; set; }

        public bool IsRead { get; set; }

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;

        public Guid? AdminId { get; set; }

        public AdminEntity? Admin { get; set; } = null!;
    }
}
