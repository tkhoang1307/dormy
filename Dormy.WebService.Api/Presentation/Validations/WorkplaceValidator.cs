using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class WorkplaceValidator
    {
        public static async Task<ApiResponse> WorkplaceRequestModelValidator(WorkplaceRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name)));
            }

            if (string.IsNullOrEmpty(model.Address))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Address)));
            }

            if (string.IsNullOrEmpty(model.Abbrevation))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Abbrevation)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> WorkplaceUpdateRequestModelValidator(WorkplaceUpdateRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name)));
            }

            if (string.IsNullOrEmpty(model.Address))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Address)));
            }

            if (string.IsNullOrEmpty(model.Abbrevation))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Abbrevation)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
