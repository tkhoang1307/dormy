using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class NotificationMapper
    {
        public NotificationModel MapToNotificationModel(NotificationEntity source)
        {
            return new NotificationModel()
            {
                Id = source.Id,
                Title = source.Title,
                Description = source.Description,
                Content = source.Content,
                Date = source.Date,
                DoB = source.User.DateOfBirth,
                Email = source.User.Email,
                Gender = source.User.Gender.ToString(),
                PhoneNumber = source.User.PhoneNumber,
                UserFullName = $"{source.User.LastName} {source.User.LastName}",
                UserId = source.UserId,
                NotificationType = source.NotificationType.ToString(),
            };
        }
    }
}
