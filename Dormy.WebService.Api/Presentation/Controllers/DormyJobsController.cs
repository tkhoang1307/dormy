using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/job")]
    [ApiController]
    public class DormyJobsController : ControllerBase
    {
        private readonly IDormyJobsService _dormyJobsService;

        public DormyJobsController(IDormyJobsService dormyJobsService)
        {
            _dormyJobsService = dormyJobsService;
        }

        [HttpPost("contract")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RunContractJob()
        {
            var response = await _dormyJobsService.ContractJob();

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("contract-extension")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RunContractExtensionJob()
        {
            var response = await _dormyJobsService.ContractExtensionJob();

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("invoice")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RunInvoiceJob()
        {
            var response = await _dormyJobsService.ỊnvoiceJob();

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
