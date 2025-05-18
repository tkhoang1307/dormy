using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.IdentityModel.Tokens;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class InvoiceValidator
    {
        public static async Task<ApiResponse> InvoiceRequestModelValidator(InvoiceRequestModel model)
        {
            //if (model?.DueDate == null)
            //{
            //    return new ApiResponse().SetUnprocessableEntity(message:
            //        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DueDate)));
            //}

            if (model?.RoomId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId)));
            }

            if (string.IsNullOrEmpty(model.Type))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Type)));
            }

            if (!Enum.TryParse(model.Type, out InvoiceTypeEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Type, nameof(InvoiceTypeEnum)));
            }

            if (result == InvoiceTypeEnum.ROOM_SERVICE_MONTHLY)
            {
                if (model.Month == null)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month)));
                }

                if (model.Year == null)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year)));
                }

                if (model.Month <= 0 || model.Month >= 13)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.InvalidMonth, nameof(model.Month)));
                }
            }

            foreach (var invoiceItem in model.InvoiceItems)
            {
                if (invoiceItem?.RoomServiceId == null)
                {
                    if (string.IsNullOrEmpty(invoiceItem?.RoomServiceName))
                    {
                        return new ApiResponse().SetUnprocessableEntity(message:
                            string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.RoomServiceName)));
                    }

                    if (invoiceItem?.Cost == null)
                    {
                        return new ApiResponse().SetUnprocessableEntity(message:
                            string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Cost)));
                    }

                    if (string.IsNullOrEmpty(invoiceItem?.Unit))
                    {
                        return new ApiResponse().SetUnprocessableEntity(message:
                            string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Unit)));
                    }
                }

                if (invoiceItem?.Quantity == null)
                {
                    return new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Quantity)));
                }

                if (invoiceItem.Quantity < 0)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(invoiceItem.Quantity)));
                }
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> GetInitialInvoiceCreationRequestModelValidator(GetInitialInvoiceCreationRequestModel model)
        {
            if (model?.RoomId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId)));
            }

            if (model?.Month == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month)));
            }

            if (model?.Year == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year)));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> InvoiceUpdationRequestModelValidator(InvoiceUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (model?.DueDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DueDate)));
            }

            if (model?.RoomId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId)));
            }

            if (string.IsNullOrEmpty(model.Type))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Type)));
            }

            if (!Enum.TryParse(model.Type, out InvoiceTypeEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Type, nameof(InvoiceTypeEnum)));
            }

            if (result == InvoiceTypeEnum.ROOM_SERVICE_MONTHLY)
            {
                if (model.Month == null)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month)));
                }

                if (model.Year == null)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year)));
                }

                if (model.Month <= 0 || model.Month >= 13)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.InvalidMonth, nameof(model.Month)));
                }
            }

            foreach (var invoiceItem in model.InvoiceItems)
            {
                if (invoiceItem?.RoomServiceId == null)
                {
                    if (string.IsNullOrEmpty(invoiceItem?.RoomServiceName))
                    {
                        return new ApiResponse().SetUnprocessableEntity(message:
                            string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.RoomServiceName)));
                    }

                    if (invoiceItem?.Cost == null)
                    {
                        return new ApiResponse().SetUnprocessableEntity(message:
                            string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Cost)));
                    }

                    if (string.IsNullOrEmpty(invoiceItem?.Unit))
                    {
                        return new ApiResponse().SetUnprocessableEntity(message:
                            string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Unit)));
                    }
                }

                if (invoiceItem?.Quantity == null)
                {
                    return new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(invoiceItem.Quantity)));
                }

                if (invoiceItem.Quantity <= 0)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(invoiceItem.Quantity)));
                }
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> InvoiceStatusUpdationRequestModelValidator(InvoiceStatusUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.Status))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Status)));
            }

            if (!Enum.TryParse(model.Status, out InvoiceStatusEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Status, nameof(InvoiceStatusEnum)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
