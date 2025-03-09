using Dormy.WebService.Api.Core.Interfaces;
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
            var result = await _overnightAbsenceService.AddOvernightAbsence(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetDetailOvernightAbsence(Guid id)
        {
            var result = await _overnightAbsenceService.GetDetailOvernightAbsence(id);
            return Ok(result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetBatchOvernightAbsence(GetBatchRequestModel model)
        {
            var result = await _overnightAbsenceService.GetOvernightAbsenceBatch(model);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/approve")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ApproveOvernightAbsence(Guid id)
        {
            var result = await _overnightAbsenceService.UpdateStatusOvernightAbsence(id, OvernightAbsenceStatusEnum.APPROVED);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/reject")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RejectOvernightAbsence(Guid id)
        {
            var result = await _overnightAbsenceService.UpdateStatusOvernightAbsence(id, OvernightAbsenceStatusEnum.REJECTED);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/cancel")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CancelOvernightAbsence(Guid id)
        {
            var result = await _overnightAbsenceService.UpdateStatusOvernightAbsence(id, OvernightAbsenceStatusEnum.CANCELLED);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/update")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateOvernightAbsence(Guid id, OvernightAbsentRequestModel model)
        {
            var result = await _overnightAbsenceService.UpdateOvernightAbsence(id, model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
