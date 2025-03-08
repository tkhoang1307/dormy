using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
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
            if (model?.DueDate == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DueDate))));
            }

            if (model?.RoomId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId))));
            }

            if (string.IsNullOrEmpty(model.Type))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Type))));
            }

            if (!Enum.TryParse(model.Type, out InvoiceTypeEnum result))
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Type, nameof(InvoiceTypeEnum))));
            }

            if (result == InvoiceTypeEnum.ROOM_SERVICE_MONTHLY)
            {
                if (model.Month == null)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month))));
                }

                if (model.Year == null)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year))));
                }

                if (model.Month <= 0 || model.Month >= 13)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.InvalidMonth, nameof(model.Month))));
                }
            }

            foreach(var invoiceItem in model.InvoiceItems)
            {
                if (invoiceItem?.RoomServiceId == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.RoomServiceId))));
                }

                if (invoiceItem?.Quantity == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Quantity))));
                }

                if (invoiceItem.Quantity <= 0)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(invoiceItem.Quantity))));
                }
            }

            var response = await _invoiceService.CreateNewInvoice(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("create-initial-invoice")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetInitialInvoiceCreation(GetInitialInvoiceCreationRequestModel model)
        {
            if (model?.RoomId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId))));
            }

            if (model.Month == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month))));
            }

            if (model.Year == null)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year))));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month))));
            }

            var response = await _invoiceService.GetInitialInvoiceCreation(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateInvoice(InvoiceUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            }

            if (model?.DueDate == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DueDate))));
            }

            if (model?.RoomId == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId))));
            }

            if (string.IsNullOrEmpty(model.Type))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Type))));
            }

            if (!Enum.TryParse(model.Type, out InvoiceTypeEnum result))
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Type, nameof(InvoiceTypeEnum))));
            }

            if (result == InvoiceTypeEnum.ROOM_SERVICE_MONTHLY)
            {
                if (model.Month == null)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month))));
                }

                if (model.Year == null)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year))));
                }

                if (model.Month <= 0 || model.Month >= 13)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.InvalidMonth, nameof(model.Month))));
                }
            }

            foreach (var invoiceItem in model.InvoiceItems)
            {
                if (invoiceItem?.RoomServiceId == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.RoomServiceId))));
                }

                if (invoiceItem?.Quantity == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Quantity))));
                }

                if (invoiceItem.Quantity <= 0)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(invoiceItem.Quantity))));
                }
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
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            }

            if (string.IsNullOrEmpty(model.Status))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Status))));
            }

            if (!Enum.TryParse(model.Status, out InvoiceStatusEnum result))
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Status, nameof(InvoiceStatusEnum))));
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
