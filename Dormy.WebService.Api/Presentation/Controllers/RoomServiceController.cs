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
            foreach(var model in models)
            {
                if (string.IsNullOrEmpty(model.RoomServiceName))
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceName))));
                }

                if (string.IsNullOrEmpty(model.Unit))
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Unit))));
                }

                if (model?.Cost == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Cost))));
                }

                if (model.Cost < 0)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message: 
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.Cost))));
                }
            }

            var result = await _roomServiceService.AddRoomServiceBatch(models);

            return StatusCode(201, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoomService(RoomServiceUpdateRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RoomServiceName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceName))));
            }

            if (string.IsNullOrEmpty(model.Unit))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Unit))));
            }

            if (model?.Cost == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Cost))));
            }

            if (model.Cost < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message: string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.Cost))));
            }

            var result = await _roomServiceService.UpdateRoomService(model);

            if (result.IsSuccess)
            {
                return StatusCode(202, result);
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
