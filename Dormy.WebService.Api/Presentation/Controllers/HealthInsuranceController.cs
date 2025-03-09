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
    public class HealthInsuranceController : ControllerBase
    {
        private readonly IHealthInsuranceService _healthInsuranceService;

        public HealthInsuranceController(IHealthInsuranceService healthInsuranceService)
        {
            _healthInsuranceService = healthInsuranceService;
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CreateHealthInsurance(HealthInsuranceRequestModel model)
        {
            var modelValidator = await HealthInsuranceValidator.HealthInsuranceRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _healthInsuranceService.AddHealthInsurance(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateHealthInsurance(HealthInsuranceUpdationRequestModel model)
        {
            var modelValidator = await HealthInsuranceValidator.HealthInsuranceUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _healthInsuranceService.UpdateHealthInsurance(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetHealthInsuranceById(Guid id)
        {
            var result = await _healthInsuranceService.GetDetailHealthInsurance(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetBatchHealthInsurances(GetBatchRequestModel model)
        {
            var result = await _healthInsuranceService.GetHealthInsuranceBatch(model);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
