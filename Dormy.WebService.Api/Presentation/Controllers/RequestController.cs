using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPut("id/{id:guid}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ApproveRequest(Guid id)
        {
            var result = await _requestService.UpdateRequestStatus(id, Models.Enums.RequestStatusEnum.APPROVED);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateRequest(Guid id, RequestRequestModel model)
        {
            var result = await _requestService.UpdateRequest(id, model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/reject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RejectRequest(Guid id)
        {
            var result = await _requestService.UpdateRequestStatus(id, Models.Enums.RequestStatusEnum.REJECTED);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CancelRequest(Guid id)
        {
            var result = await _requestService.UpdateRequestStatus(id, Models.Enums.RequestStatusEnum.CANCELLED);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
