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
    }
}
