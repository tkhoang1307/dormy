using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ContractValidator
    {
        public static async Task<ApiResponse> ApproveOrRejectContractRequestModelValidator(ApproveOrRejectContractRequestModel model)
        {
            if (model?.IsAccepted == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsAccepted)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ContractExtensionRequestModelValidator(ContractExtensionRequestModel model)
        {
            //if (model?.ContractId == null)
            //{
            //    return new ApiResponse().SetUnprocessableEntity(message:
            //        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ContractId)));
            //}

            if (model?.StartDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.StartDate)));
            }

            if (model?.EndDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.EndDate)));
            }

            if (model.StartDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.StartDate)));
            }

            if (model.EndDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.EndDate)));
            }

            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(model.StartDate, model.EndDate, false))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.StartDateMustBeLessThanEndDate));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ContractExtensionUpdationRequestModelValidator(ContractExtensionUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (model?.StartDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.StartDate)));
            }

            if (model?.EndDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.EndDate)));
            }

            if (model.StartDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.StartDate)));
            }

            if (model.EndDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.EndDate)));
            }

            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(model.StartDate, model.EndDate, false))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.StartDateMustBeLessThanEndDate));
            }

            return new ApiResponse().SetOk();
        }
    }
}
