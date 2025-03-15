using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ViolationValidator
    {
        public static async Task<ApiResponse> ViolationRequestModelValidator(ViolationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Description)));
            }

            if (model?.ViolationDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ViolationDate)));
            }

            if (model?.Penalty == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Penalty)));
            }

            if (model?.UserId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.UserId)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ViolationUpdationRequestModelValidator(ViolationUpdationRequestModel model)
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

            if (model?.ViolationDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ViolationDate)));
            }

            if (model?.Penalty == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Penalty)));
            }

            if (model?.UserId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.UserId)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
