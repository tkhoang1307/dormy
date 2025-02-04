using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
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

        [HttpPost("batch/create/buildingId/{buildingId:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomBatch(Guid buildingId, [FromBody] List<RoomCreationRequestModel> rooms)
        {
            foreach(var room in rooms)
            {
                if (room?.FloorNumber == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.FloorNumber))));
                }

                if (room?.RoomTypeId == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.RoomTypeId))));
                }

                if (room?.TotalRoomsWantToCreate == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.TotalRoomsWantToCreate))));
                }

                if (room.FloorNumber < 0)
                {
                    return StatusCode(412, new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(room.FloorNumber))));
                }

                if (room.TotalRoomsWantToCreate <= 0)
                {
                    return StatusCode(412, new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(room.TotalRoomsWantToCreate))));
                }
            }
            var response = await _roomService.CreateRoomBatch(rooms, buildingId);
            return StatusCode(201, response);
        }

        [HttpGet("batch/buildingId/{buildingId:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetRoomsByBuildingId(Guid buildingId)
        {
            var response = await _roomService.GetRoomsByBuildingId(buildingId);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var response = await _roomService.GetRoomById(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
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

        [HttpDelete("delete/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteBatch([FromBody] List<Guid> ids)
        {
            var response = await _roomService.SoftDeleteRoomBatch(ids);
            return response.IsSuccess ? Ok(response) : NotFound(response.Result);
        }
    }
}
