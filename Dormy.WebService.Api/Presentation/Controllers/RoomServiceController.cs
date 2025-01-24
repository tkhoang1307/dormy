using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomServiceController : ControllerBase
    {
        private readonly IRoomServiceService _roomServiceService;

        public RoomServiceController(IRoomServiceService roomServiceService)
        {
            _roomServiceService = roomServiceService;
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetRoomServiceById(Guid id)
        {
            var result = await _roomServiceService.GetRoomSeviceSingle(id);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(id);
        }

        [HttpPost("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetRoomServiceBatch(GetBatchRequestModel model)
        {
            var result = await _roomServiceService.GetRoomServiceBatch(model);

            return Ok(result);
        }

        [HttpPost("batch/create")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomServiceBatch(List<RoomServiceRequestModel> models)
        {
            var result = await _roomServiceService.AddRoomServiceBatch(models);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoomService(RoomServiceUpdateRequestModel model)
        {
            var result = await _roomServiceService.UpdateRoomService(model);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(model.Id);
        }

        [HttpDelete("batch/soft-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteRoomService(List<Guid> ids)
        {
            var result = await _roomServiceService.SoftDeleteRoomServiceBatch(ids);

            return Ok(result);
        }
    }
}
