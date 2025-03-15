using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/violation")]
    [ApiController]
    public class ViolationController : ControllerBase
    {
        private readonly IViolationService _violationService;
        public ViolationController(IViolationService violationService)
        {
            _violationService = violationService;
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetViolation(Guid id)
        {
            var result = await _violationService.GetSingleViolation(id);
            return Ok(result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetViolationBatch(GetBatchRequestModel model)
        {
            var result = await _violationService.GetViolationBatch(model);
            return Ok(result);
        }

        [HttpPut()]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateViolation(ViolationUpdationRequestModel model)
        {
            var modelValidator = await ViolationValidator.ViolationUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _violationService.UpdateViolation(model);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateViolation(ViolationRequestModel model)
        {
            var modelValidator = await ViolationValidator.ViolationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _violationService.CreateViolation(model);
            return Ok(result);
        }

        [HttpDelete("id/{id:guid}/soft-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> DeleteViolation(Guid id)
        {
            var result = await _violationService.SoftDeleteViolation(id);
            return Ok(result);
        }
    }
}
