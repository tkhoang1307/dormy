using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/room-service")]
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
            var response = await _roomServiceService.GetRoomSeviceSingle(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetRoomServiceBatch(GetBatchRequestModel model)
        {
            var response = await _roomServiceService.GetRoomServiceBatch(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("batch/create")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomServiceBatch(List<RoomServiceRequestModel> models)
        {
            foreach(var model in models)
            {
                var modelValidator = await RoomServiceValidator.RoomServiceRequestModelValidator(model);
                if (!modelValidator.IsSuccess)
                {
                    return StatusCode((int)modelValidator.StatusCode, modelValidator);
                }
            }

            var response = await _roomServiceService.AddRoomServiceBatch(models);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoomService(RoomServiceUpdateRequestModel model)
        {
            var modelValidator = await RoomServiceValidator.RoomServiceUpdateRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _roomServiceService.UpdateRoomService(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("batch/soft-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteRoomService(List<Guid> ids)
        {
            var response = await _roomServiceService.SoftDeleteRoomServiceBatch(ids);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("room-service-type/all")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetAllRoomServiceTypes()
        {
            var roomServiceTypes = RoomServiceTypeEnumHelper.GetAllRoomServiceTypes();

            return Ok(roomServiceTypes);
        }
    }
}
