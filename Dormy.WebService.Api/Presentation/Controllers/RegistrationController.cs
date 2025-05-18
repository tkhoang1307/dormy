using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IContractExtensionService _contractExtensionService;
        private readonly IRoomService _roomService;
        public RegistrationController(IContractService contractService, 
                                      IContractExtensionService contractExtensionService,
                                      IRoomService roomService)
        {
            _contractService = contractService;
            _contractExtensionService = contractExtensionService;
            _roomService = roomService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            var modelValidator = await RegistrationValidator.RegisterRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _contractService.Register(model);
            if (result.IsSuccess)
            {
                await _contractService.SendContractEmail(((RegistrationResponseModel)result.Result).ContractId, false);
            }
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetRegistrationAccommodationBatch()
        {
            var result = await _contractExtensionService.GetRegistrationAccommodationBatch();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("register-initial-data")]
        public async Task<IActionResult> GetInitialRegistrationData()
        {
            var result = await _contractService.GetInitialRegistrationData();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("search-buildings-and-rooms")]
        public async Task<IActionResult> SearchBuildingsAndRoomsByGenderAndRoomType(SearchBuildingAndRoomRequestModel model)
        {
            var result = await _contractService.SearchBuildingsAndRoomsByGenderAndRoomType(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("room/{id:guid}")]
        public async Task<IActionResult> GetRoomSumaryById(Guid id)
        {
            var result = await _roomService.GetRoomSumaryById(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
