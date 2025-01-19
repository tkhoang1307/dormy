using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("batch/buildingId/{buildingId:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomBatch(Guid buildingId, [FromBody] List<RoomRequestModel> rooms)
        {
            var response = await _roomService.CreateRoomBatch(rooms, buildingId);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoom(RoomUpdateRequestModel room)
        {
            var response = await _roomService.UpdateRoom(room);
            return response.IsSuccess ? Ok(response) : NotFound(response.Result);
        }

        [HttpPut("status")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoomStatus(RoomUpdateStatusRequestModel room)
        {
            var response = await _roomService.UpdateRoomStatus(room);
            return response.IsSuccess ? Ok(response) : NotFound(response.Result);
        }

        [HttpDelete("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var response = await _roomService.SoftDeleteRoom(id);
            return response.IsSuccess ? Ok(response) : NotFound(response.Result);
        }
    }
}
