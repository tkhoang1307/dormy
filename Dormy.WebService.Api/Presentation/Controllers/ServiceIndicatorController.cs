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
    public class ServiceIndicatorController : ControllerBase
    {
        private readonly IServiceIndicatorService _serviceIndicatorService;

        public ServiceIndicatorController(IServiceIndicatorService serviceIndicatorService)
        {
            _serviceIndicatorService = serviceIndicatorService;
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateServiceIndicator(ServiceIndicatorRequestModel model)
        {
            if (model?.RoomServiceId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceId))));
            }

            if (model?.RoomId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId))));
            }

            if (model?.Month == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month))));
            }

            if (model?.Year == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year))));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month))));
            }

            if (model?.NewIndicator == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NewIndicator))));
            }

            if (model.NewIndicator < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.NewIndicator))));
            }

            if (model?.OldIndicator != null)
            {
                if (model.OldIndicator < 0)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.OldIndicator))));
                }

                if (model.OldIndicator > model.NewIndicator)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(model.OldIndicator), nameof(model.NewIndicator))));
                }
            }

            var response = await _serviceIndicatorService.AddServiceIndicator(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("create/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateBatchServiceIndicator(ServiceIndicatorCreationBatchRequestModel model)
        {
            if (model?.RoomId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId))));
            }

            if (model?.Month == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month))));
            }

            if (model?.Year == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year))));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month))));
            }

            foreach (var roomServiceIndicator in model.RoomServiceIndicators)
            {
                if (roomServiceIndicator?.RoomServiceId == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(roomServiceIndicator.RoomServiceId))));
                }

                if (roomServiceIndicator?.NewIndicator == null)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(roomServiceIndicator.NewIndicator))));
                }

                if (roomServiceIndicator.NewIndicator < 0)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(roomServiceIndicator.NewIndicator))));
                }

                if (roomServiceIndicator?.OldIndicator != null && roomServiceIndicator.OldIndicator > roomServiceIndicator.NewIndicator)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(roomServiceIndicator.OldIndicator), nameof(roomServiceIndicator.NewIndicator))));
                }
            }
            var response = await _serviceIndicatorService.AddBatchServiceIndicators(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateServiceIndicator(ServiceIndicatorUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            }

            if (model?.RoomServiceId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceId))));
            }

            if (model?.RoomId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId))));
            }

            if (model?.Month == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month))));
            }

            if (model?.Year == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year))));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month))));
            }

            if (model?.NewIndicator == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NewIndicator))));
            }

            if (model.NewIndicator < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.NewIndicator))));
            }

            if (model?.OldIndicator != null)
            {
                if (model.OldIndicator < 0)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.OldIndicator))));
                }

                if (model.OldIndicator > model.NewIndicator)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(model.OldIndicator), nameof(model.NewIndicator))));
                }
            }

            var response = await _serviceIndicatorService.UpdateServiceIndicator(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetServiceIndicatorById(Guid id)
        {
            var result = await _serviceIndicatorService.GetDetailServiceIndicatorById(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetBatchServiceIndicators(GetBatchServiceIndicatorRequestModel model)
        {
            var result = await _serviceIndicatorService.GetServiceIndicatorBatch(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("hard-delete/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteRoomService(List<Guid> ids)
        {
            var result = await _serviceIndicatorService.HardDeleteBatchServiceIndicators(ids);

            return StatusCode((int)result.StatusCode, result);
        }
    }

}
