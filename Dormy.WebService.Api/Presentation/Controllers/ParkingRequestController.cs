using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingRequestController : ControllerBase
    {
        private readonly IParkingRequestService _parkingRequestService;

        public ParkingRequestController(IParkingRequestService parkingSpotService)
        {
            _parkingRequestService = parkingSpotService;
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CreateNewParkingRequest(ParkingRequestModel model)
        {
            var modelValidator = await ParkingRequestValidator.ParkingRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _parkingRequestService.CreateParkingRequest(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateParkingRequest(UpdateParkingRequestModel model)
        {
            var modelValidator = await ParkingRequestValidator.UpdateParkingRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _parkingRequestService.UpdateParkingRequest(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("admin/approve-or-reject/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> AcceptParkingRequest(ApproveOrRejectParkingRequestModel model)
        {
            var modelValidator = await ParkingRequestValidator.ApproveOrRejectParkingRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var payload = new ParkingRequestStatusModel();

            if (model.IsAccepted)
            {
                payload.Id = model.Id;
                payload.Status = RequestStatusEnum.APPROVED;
            }
            else
            {
                payload.Id = model.Id;
                payload.Status = RequestStatusEnum.REJECTED;
            }

            var response = await _parkingRequestService.UpdateParkingRequestStatus(payload);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("user/cancel/id/{id:guid}")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CancelParkingRequest(Guid id)
        {
            var response = await _parkingRequestService.UpdateParkingRequestStatus(
                new()
                {
                    Id = id,
                    Status = RequestStatusEnum.CANCELLED
                });

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetParkingRequestById(Guid id)
        {
            var result = await _parkingRequestService.GetSingleParkingRequest(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetParkingRequestBatch(GetBatchRequestModel model)
        {
            var result = await _parkingRequestService.GetParkingRequestBatch(model.Ids, model.IsGetAll);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("soft-delete/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteParkingSpot(Guid id)
        {
            var result = await _parkingRequestService.SoftDeleteParkingRequest(id);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
