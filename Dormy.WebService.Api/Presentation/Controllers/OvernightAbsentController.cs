using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/overnight-absence")]
    [ApiController]
    public class OvernightAbsentController : ControllerBase
    {
        private readonly IOvernightAbsenceService _overnightAbsenceService;

        public OvernightAbsentController(IOvernightAbsenceService overnightAbsenceService)
        {
            _overnightAbsenceService = overnightAbsenceService;
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> AddOvernightAbsence(OvernightAbsentRequestModel model)
        {
            var modelValidator = await OvernightAbsenceValidatior.OvernightAbsentRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _overnightAbsenceService.AddOvernightAbsence(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetDetailOvernightAbsence(Guid id)
        {
            var result = await _overnightAbsenceService.GetDetailOvernightAbsence(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetBatchOvernightAbsence(GetBatchRequestModel model)
        {
            var result = await _overnightAbsenceService.GetOvernightAbsenceBatch(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/approve-or-reject")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ApproveOrRejectOvernightAbsence(OvernightAbsentApproveOrRejectRequestModel model)
        {
            var modelValidator = await OvernightAbsenceValidatior.OvernightAbsentApproveOrRejectRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var status = OvernightAbsenceStatusEnum.REJECTED;
            if (model.IsApproved)
            {
                status = OvernightAbsenceStatusEnum.APPROVED;
            }

            var result = await _overnightAbsenceService.UpdateStatusOvernightAbsence(model.Id, status);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/cancel")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CancelOvernightAbsence(Guid id)
        {
            var result = await _overnightAbsenceService.UpdateStatusOvernightAbsence(id, OvernightAbsenceStatusEnum.CANCELLED);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut()]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateOvernightAbsence(OvernightAbsentUpdationRequestModel model)
        {
            var modelValidator = await OvernightAbsenceValidatior.OvernightAbsentUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _overnightAbsenceService.UpdateOvernightAbsence(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("id/{id:guid}/soft-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteOvernightAbsence(Guid id)
        {
            var result = await _overnightAbsenceService.SoftDeleteOvernightAbsence(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
