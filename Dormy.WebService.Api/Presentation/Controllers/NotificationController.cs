using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var response = await _notificationService.GetNotifications();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("read/id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> ReadNotification(Guid id)
        {
            var result = await _notificationService.ReadNotification(id);
            return StatusCode((int)result.StatusCode, result.Result);
        }

        [HttpPost("post-announcement")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateAnnouncement(AnnouncementCreationRequestModel model)
        {
            var modelValidator = await NotificationValidator.AnnouncementCreationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _notificationService.CreateAnnouncement(model);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
