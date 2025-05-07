using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [HttpPost("run-all-services")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> RunJobForAllServices()
        {
            var countServiceRunJob = 3;
            var responseỊnvoiceJob = await _dormyJobsService.ỊnvoiceJob();
            var responseContractJob = await _dormyJobsService.ContractJob();
            var responseContractExtensionJob = await _dormyJobsService.ContractExtensionJob();

            var countSuccess = 0;
            var errorMessage = "Run error in services: ";
            var result = "Run successfully in services: ";
            if (responseỊnvoiceJob.IsSuccess)
            {
                countSuccess++;
                result = result + "Invoice Job, ";

            }
            else
            {
                errorMessage = errorMessage + "Invoice Job, ";
            }
            if (responseContractJob.IsSuccess)
            {
                countSuccess++;
                result = result + "Contract Job, ";

            }
            else
            {
                errorMessage = errorMessage + "Contract Job, ";
            }
            if (responseContractExtensionJob.IsSuccess)
            {
                countSuccess++;
                result = result + "Contract Extension Job, ";

            }
            else
            {
                errorMessage = errorMessage + "Contract Extension Job, ";
            }

            errorMessage = errorMessage.TrimEnd(' ', ',', ':') + ".";
            result = result.TrimEnd(' ', ',', ':') + ".";

            var response = new ApiResponse().SetApiResponse(
                statusCode: countSuccess == countServiceRunJob ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable,
                isSuccess: countSuccess == countServiceRunJob ? true : false,
                message: countSuccess == countServiceRunJob ? null : errorMessage,
                result: countSuccess == countServiceRunJob ? result : null
                );
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
