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
            if (string.IsNullOrEmpty(model.InsuranceCardNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.InsuranceCardNumber))));
            }

            if (string.IsNullOrEmpty(model.RegisteredHospital))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RegisteredHospital))));
            }

            if (model?.ExpirationDate == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ExpirationDate))));
            }

            var result = await _healthInsuranceService.AddHealthInsurance(model);

            return StatusCode(201, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateHealthInsurance(HealthInsuranceUpdationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.InsuranceCardNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.InsuranceCardNumber))));
            }

            if (string.IsNullOrEmpty(model.RegisteredHospital))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RegisteredHospital))));
            }

            if (model?.ExpirationDate == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ExpirationDate))));
            }

            var result = await _healthInsuranceService.UpdateHealthInsurance(model);

            return StatusCode(202, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetHealthInsuranceById(Guid id)
        {
            var result = await _healthInsuranceService.GetDetailHealthInsurance(id);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetBatchHealthInsurances(GetBatchRequestModel model)
        {
            var result = await _healthInsuranceService.GetHealthInsuranceBatch(model);

            return Ok(result);
        }
    }
}
