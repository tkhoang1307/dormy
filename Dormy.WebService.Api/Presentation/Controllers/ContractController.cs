using Dormy.WebService.Api.ApplicationLogic;
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
    [Route("api/contract")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IUserContextService _userContextService;
        public ContractController(IContractService contractService, IUserContextService userContextService)
        {
            _contractService = contractService;
            _userContextService = userContextService;
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateNewContract(ContractRequestModel model)
        {
            if (_userContextService.UserId != model.UserId)
            {
                var response = new ApiResponse().SetForbidden(message: ErrorMessages.AccountDoesNotHavePermissionCreateContract);
                return StatusCode((int)response.StatusCode, response);
            }
            var result = await _contractService.AddNewContract(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContract(Guid id)
        {
            var response = await _contractService.GetSingleContract(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> GetContractBatch(GetBatchRequestModel model)
        {
            var response = await _contractService.GetContractBatch(model);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("id/{id:guid}/active")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ActiveContract(Guid id)
        {
            var response = await _contractService.UpdateContractStatus(id, ContractStatusEnum.ACTIVE);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("id/{id:guid}/approve-or-reject")]
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

        [HttpPut("id/{id:guid}/terminate")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
        public async Task<IActionResult> TerminateContract(Guid id)
        {
            var response = await _contractService.UpdateContractStatus(id, ContractStatusEnum.TERMINATED);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("id/{id:guid}/expire")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ExpiredContract(Guid id)
        {
            var response = await _contractService.UpdateContractStatus(id, ContractStatusEnum.EXPIRED);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("all-room-types")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> GetAllRoomTypesData()
        {
            var result = await _contractService.GetAllRoomTypesData();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("initial-create-contract-data")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> GetInitialCreateContractData()
        {
            var result = await _contractService.GetInitialCreateContractData();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}/initial-extend-contract-data")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> GetInitialExtendContractData(Guid id)
        {
            var result = await _contractService.GetInitialExtendContractData(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
