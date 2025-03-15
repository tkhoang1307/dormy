using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CreateRequest(RequestRequestModel model)
        {
            var modelValidator = await RequestValidator.RequestRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _requestService.CreateRequest(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetRequests(GetBatchRequestModel model)
        {
            var result = await _requestService.GetRequests(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetRequest(Guid id)
        {
            var result = await _requestService.GetSingleRequest(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/approve-or-reject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ApproveOrRejectRequest(RequestApproveOrRejectRequestModel model)
        {
            var modelValidator = await RequestValidator.RequestApproveOrRejectRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            RequestStatusEnum status = RequestStatusEnum.REJECTED;
            if (model.IsApproved)
            {
                status = RequestStatusEnum.APPROVED;
            }    
            var result = await _requestService.UpdateRequestStatus(model.Id, status);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateRequest(RequestUpdationRequestModel model)
        {
            var modelValidator = await RequestValidator.RequestUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _requestService.UpdateRequest(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CancelRequest(Guid id)
        {
            var result = await _requestService.UpdateRequestStatus(id, RequestStatusEnum.CANCELLED);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
