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

        [HttpPut("active/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ActiveContract(Guid id)
        {
            var result = await _contractService.UpdateContractStatus(id, ContractStatusEnum.ACTIVE);
            return Ok(result);
        }

        [HttpPut("admin/approve-or-reject/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ApproveOrRejectRegisterContract(Guid id, [FromBody] ApproveOrRejectContractRequestModel model)
        {
            var modelValidator = await ContractValidator.ApproveOrRejectContractRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var payload = new ContractUpdationStatusRequestModel();

            if (model.IsAccepted)
            {
                payload.Id = id;
                payload.Status = ContractStatusEnum.WAITING_PAYMENT;
            }
            else
            {
                payload.Id = id;
                payload.Status = ContractStatusEnum.REJECTED;
            }

            var response = await _contractService.UpdateContractStatus(payload.Id, payload.Status);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("terminate/id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> TerminateContract(Guid id)
        {
            var result = await _contractService.UpdateContractStatus(id, ContractStatusEnum.TERMINATED);
            return Ok(result);
        }

        [HttpPut("expired/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ExpiredContract(Guid id)
        {
            var result = await _contractService.UpdateContractStatus(id, ContractStatusEnum.EXPIRED);
            return Ok(result);
        }
    }
}
