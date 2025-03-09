using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContract(Guid id)
        {
            var result = await _contractService.GetSingleContract(id);
            return Ok(result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContractBatch(GetBatchRequestModel model)
        {
            var result = await _contractService.GetContractBatch(model);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/active")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ActiveContract(Guid id)
        {
            var result = await _contractService.UpdateContractStatus(id, Models.Enums.ContractStatusEnum.ACTIVE);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/accept")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> AcceptRegister(Guid id)
        {
            var result = await _contractService.UpdateContractStatus(id, Models.Enums.ContractStatusEnum.WAITING_PAYMENT);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/reject")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> RejectContract(Guid id)
        {
            var result = await _contractService.UpdateContractStatus(id, Models.Enums.ContractStatusEnum.REJECTED);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/terminate")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> TerminateContract(Guid id)
        {
            var result = await _contractService.UpdateContractStatus(id, Models.Enums.ContractStatusEnum.TERMINATED);
            return Ok(result);
        }
    }
}
