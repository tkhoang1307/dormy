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
            var modelValidator = await ServiceIndicatorValidator.ServiceIndicatorRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _serviceIndicatorService.AddServiceIndicator(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("create/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateBatchServiceIndicator(ServiceIndicatorCreationBatchRequestModel model)
        {
            var modelValidator = await ServiceIndicatorValidator.ServiceIndicatorCreationBatchRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _serviceIndicatorService.AddBatchServiceIndicators(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateServiceIndicator(ServiceIndicatorUpdationRequestModel model)
        {
            var modelValidator = await ServiceIndicatorValidator.ServiceIndicatorUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
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
