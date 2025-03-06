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

            if (model?.OldIndicator == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.OldIndicator))));
            }

            if (model?.NewIndicator == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NewIndicator))));
            }

            if (model.OldIndicator < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.OldIndicator))));
            }

            if (model.NewIndicator < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.NewIndicator))));
            }

            if (model.OldIndicator > model.NewIndicator)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(model.OldIndicator), nameof(model.NewIndicator))));
            }

            var response = await _serviceIndicatorService.AddServiceIndicator(model);

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

            if (model?.OldIndicator == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.OldIndicator))));
            }

            if (model?.NewIndicator == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NewIndicator))));
            }

            if (model.OldIndicator < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.OldIndicator))));
            }

            if (model.NewIndicator < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.NewIndicator))));
            }

            if (model.OldIndicator > model.NewIndicator)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(model.OldIndicator), nameof(model.NewIndicator))));
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
    }

}
