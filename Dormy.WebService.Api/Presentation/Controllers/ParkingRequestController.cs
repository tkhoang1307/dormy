using Dormy.WebService.Api.Core.Constants;
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
            if (string.IsNullOrEmpty(model.Description))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Description))));
            }

            if (model?.ParkingSpotId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotId))));
            }

            if (model?.VehicleId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.VehicleId))));
            }

            var result = await _parkingRequestService.CreateParkingRequest(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateParkingRequest(UpdateParkingRequestModel model)
        {
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Description))));
            }

            if (model?.ParkingSpotId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotId))));
            }

            var result = await _parkingRequestService.UpdateParkingRequest(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("admin/approve-or-reject/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> AcceptParkingRequest(ApproveOrRejectParkingRequestModel model)
        {
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            }

            if (model?.IsAccepted == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsAccepted))));
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
