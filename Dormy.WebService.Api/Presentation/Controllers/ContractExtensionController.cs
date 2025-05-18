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
        private readonly IContractService _contractService;
        public ContractExtensionController(IContractExtensionService contractExtensionService, IContractService contractService)
        {
            _contractExtensionService = contractExtensionService;
            _contractService = contractService;
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
            return StatusCode((int)result.StatusCode, result);
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
            if (result.IsSuccess)
            {
                await _contractService.SendContractEmail((Guid)result.Result);
            }
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContract(Guid id)
        {
            var result = await _contractExtensionService.GetSingleContractExtensionById(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContractBatch(GetBatchRequestModel model)
        {
            var result = await _contractExtensionService.GetContractExtensionBatch(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/active")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ActiveContractExtension(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, ContractExtensionStatusEnum.ACTIVE);
            if (result.IsSuccess)
            {
                await _contractService.SendContractEmail(id);
            }
            return StatusCode((int)result.StatusCode, result);
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
            if (response.IsSuccess)
            {
                await _contractService.SendContractEmail(id);
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("id/{id:guid}/expired")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ExpiredContractExtension(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, ContractExtensionStatusEnum.EXPIRED);
            if (result.IsSuccess)
            {
                await _contractService.SendContractEmail(id);
            }
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("id/{id:guid}/terminate")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> TerminateContract(Guid id)
        {
            var result = await _contractExtensionService.UpdateContractExtensionStatus(id, ContractExtensionStatusEnum.TERMINATED);
            if (result.IsSuccess)
            {
                await _contractService.SendContractEmail(id);
            }
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
