using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
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

                if (string.IsNullOrEmpty(model.RoomServiceType))
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceType))));
                }

                if (!Enum.TryParse(model.RoomServiceType, out RoomServiceTypeEnum result))
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.RoomServiceType, nameof(RoomServiceTypeEnum))));
                }

                if (model?.IsServiceIndicatorUsed == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsServiceIndicatorUsed))));
                }
            }

            var response = await _roomServiceService.AddRoomServiceBatch(models);

            return StatusCode((int)response.StatusCode, response);
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

            if (string.IsNullOrEmpty(model.RoomServiceType))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceType))));
            }

            if (!Enum.TryParse(model.RoomServiceType, out RoomServiceTypeEnum result))
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.RoomServiceType, nameof(RoomServiceTypeEnum))));
            }

            if (model?.IsServiceIndicatorUsed == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsServiceIndicatorUsed))));
            }

            var response = await _roomServiceService.UpdateRoomService(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("soft-delete/batch")]
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
