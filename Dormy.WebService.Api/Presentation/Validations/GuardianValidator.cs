using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class GuardianValidator
    {
        public static async Task<ApiResponse> GuardianRequestModelValidator(GuardianRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name)));
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.PhoneNumber)));
            }

            if (string.IsNullOrEmpty(model.Address))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Address)));
            }

            if (string.IsNullOrEmpty(model.RelationshipToUser))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RelationshipToUser)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> GuardianUpdationRequestModelValidator(GuardianUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name)));
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.PhoneNumber)));
            }

            if (string.IsNullOrEmpty(model.Address))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Address)));
            }

            if (string.IsNullOrEmpty(model.RelationshipToUser))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RelationshipToUser)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
