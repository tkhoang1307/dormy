using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IContractService _contractService;
        public RegistrationController(IContractService contractService)
        {
            _contractService = contractService;
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
    }
}
