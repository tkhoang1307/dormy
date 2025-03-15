using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/room-type-service")]
    [ApiController]
    public class RoomTypeServiceController : ControllerBase
    {
        private readonly IRoomTypeServiceService _roomTypeServiceService;

        public RoomTypeServiceController(IRoomTypeServiceService roomTypeServiceService)
        {
            _roomTypeServiceService = roomTypeServiceService;
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomTypeService([FromBody] RoomTypeServiceCreationRequestModel model)
        {
            var response = await _roomTypeServiceService.AddRoomTypeService(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RemoveRoomTypeService([FromBody] RoomTypeServiceDeletionRequestModel model)
        {
            var response = await _roomTypeServiceService.RemoveRoomTypeService(model);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
