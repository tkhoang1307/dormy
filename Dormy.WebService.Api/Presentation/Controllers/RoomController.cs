using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("buildingId/{buildingId:guid}/batch/create")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomBatch(Guid buildingId, [FromBody] List<RoomCreationRequestModel> rooms)
        {
            foreach(var room in rooms)
            {
                var modelValidator = await RoomValidator.RoomCreationRequestModelValidator(room);
                if (!modelValidator.IsSuccess)
                {
                    return StatusCode((int)modelValidator.StatusCode, modelValidator);
                }
            }
            var response = await _roomService.CreateRoomBatch(rooms, buildingId);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("buildingId/{buildingId:guid}/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetRoomsByBuildingId(Guid buildingId)
        {
            var response = await _roomService.GetRoomsByBuildingId(buildingId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var response = await _roomService.GetRoomById(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoom(RoomUpdateRequestModel room)
        {
            var modelValidator = await RoomValidator.RoomUpdateRequestModelValidator(room);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _roomService.UpdateRoom(room);
            
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("update/status")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoomStatus(RoomUpdateStatusRequestModel room)
        {
            var modelValidator = await RoomValidator.RoomUpdateStatusRequestModelValidator(room);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _roomService.UpdateRoomStatus(room);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("id/{id:guid}/soft-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var response = await _roomService.SoftDeleteRoom(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("batch/soft-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteBatch([FromBody] List<Guid> ids)
        {
            var response = await _roomService.SoftDeleteRoomBatch(ids);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
