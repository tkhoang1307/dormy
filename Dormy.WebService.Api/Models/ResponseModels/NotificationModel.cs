using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class NotificationModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public Guid UserId { get; set; }

        public string UserFullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DoB { get; set; }

        public string Gender { get; set; } = GenderEnum.FEMALE.ToString();

        public string NotificationType { get; set; } = string.Empty;
    }
}
