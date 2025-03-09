using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractExtensionController : ControllerBase
    {
        private readonly IContractExtensionService _contractExtensionService;
        public ContractExtensionController(IContractExtensionService contractService)
        {
            _contractExtensionService = contractService;
        }

        [HttpPut("id/{id:guid}")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateContractExtension(Guid id, ContractExtensionRequestModel model)
        {
            var result = await _contractExtensionService.UpdateContractExtension(id, model);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CreateContractExtension(ContractExtensionRequestModel model)
        {
            var result = await _contractExtensionService.CreateContractExtension(model);
            return Ok(result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContract(Guid id)
        {
            var result = await _contractExtensionService.GetSingleContractExtensionById(id);
            return Ok(result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContractBatch(GetBatchRequestModel model)
        {
            var result = await _contractExtensionService.GetContractExtensionBatch(model);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/active")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ActiveContractExtension(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, Models.Enums.ContractExtensionStatusEnum.ACTIVE);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/accept")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> AcceptContractExtension(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, Models.Enums.ContractExtensionStatusEnum.WAITING_PAYMENT);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/reject")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RejectContract(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, Models.Enums.ContractExtensionStatusEnum.REJECTED);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/expired")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ExpiredContractExtension(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, Models.Enums.ContractExtensionStatusEnum.EXPIRED);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/terminate")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> TerminateContract(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, Models.Enums.ContractExtensionStatusEnum.TERMINATED);
            return Ok(result);
        }
    }
}
