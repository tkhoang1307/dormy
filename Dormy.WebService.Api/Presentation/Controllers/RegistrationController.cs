using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IContractService _contractService;
        public RegistrationController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            var result = await _contractService.Register(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("requestId/{requestId:guid}/userId/{userId:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRegistrationByRequestId(Guid requestId, Guid? userId)
        {
            var result = await _contractService.GetRegistrationByRequestId(requestId: requestId, userId: userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch/userId/{userId:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRegistrationBatchByRequestIds(GetBatchRequestModel model, Guid? userId)
        {
            var result = await _contractService.GetRegistrationBatchByRequestIds(model.Ids, userId: userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("requestId/{requestId:guid}")]
        [Authorize(Roles = Role.USER)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRegistrationByRequestId(Guid requestId, RegisterUpdateRequestModel model)
        {
            var result = await _contractService.UpdateRegistration(requestId, model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
