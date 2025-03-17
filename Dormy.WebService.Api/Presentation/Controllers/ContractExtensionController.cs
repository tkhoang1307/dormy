using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/contract-extension")]
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
        public async Task<IActionResult> UpdateContractExtension(ContractExtensionUpdationRequestModel model)
        {
            var modelValidator = await ContractValidator.ContractExtensionUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _contractExtensionService.UpdateContractExtension(model);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CreateContractExtension(ContractExtensionRequestModel model)
        {
            var modelValidator = await ContractValidator.ContractExtensionRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

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
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, ContractExtensionStatusEnum.ACTIVE);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/approve-or-reject")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ApproveOrRejectContractExtension(Guid id, [FromBody] ApproveOrRejectContractRequestModel model)
        {
            var modelValidator = await ContractValidator.ApproveOrRejectContractRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var status = ContractExtensionStatusEnum.REJECTED;
            if (model.IsAccepted)
            {
                status = ContractExtensionStatusEnum.WAITING_PAYMENT;
            }

            var response = await _contractExtensionService.UpdateContractExtensionStatus(id, status);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("id/{id:guid}/expired")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ExpiredContractExtension(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, ContractExtensionStatusEnum.EXPIRED);
            return Ok(result);
        }

        [HttpPut("id/{id:guid}/terminate")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> TerminateContract(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, ContractExtensionStatusEnum.TERMINATED);
            return Ok(result);
        }
    }
}
