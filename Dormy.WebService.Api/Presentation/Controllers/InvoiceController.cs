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
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateInvoice(InvoiceRequestModel model)
        {
            var modelValidator = await InvoiceValidator.InvoiceRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _invoiceService.CreateNewInvoice(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("create-initial-invoice")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetInitialInvoiceCreation(GetInitialInvoiceCreationRequestModel model)
        {
            var modelValidator = await InvoiceValidator.GetInitialInvoiceCreationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _invoiceService.GetInitialInvoiceCreation(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateInvoice(InvoiceUpdationRequestModel model)
        {
            var modelValidator = await InvoiceValidator.InvoiceUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _invoiceService.UpdateInvoice(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetSingleInvoice(Guid id)
        {
            var response = await _invoiceService.GetInvoiceById(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetInvoiceBatch([FromBody] GetBatchInvoiceRequestModel request)
        {
            var response = await _invoiceService.GetInvoiceBatch(request);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("update/status")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> UpdateInvoiceStatus([FromBody] InvoiceStatusUpdationRequestModel model)
        {
            var modelValidator = await InvoiceValidator.InvoiceStatusUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _invoiceService.UpdateInvoiceStatus(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("hard-delete/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> HardDeleteInvoiceById(Guid id)
        {
            var response = await _invoiceService.HardDeleteInvoiceById(id);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
