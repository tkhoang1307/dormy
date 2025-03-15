using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class RequestValidator
    {
        public static async Task<ApiResponse> RequestRequestModelValidator(RequestRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Description)));
            }

            if (string.IsNullOrEmpty(model.RequestType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RequestType)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> RequestUpdationRequestModelValidator(RequestUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Description)));
            }

            if (string.IsNullOrEmpty(model.RequestType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RequestType)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> RequestApproveOrRejectRequestModelValidator(RequestApproveOrRejectRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (model?.IsApproved == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsApproved)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
