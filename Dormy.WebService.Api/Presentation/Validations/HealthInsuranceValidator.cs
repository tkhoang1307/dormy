using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class HealthInsuranceValidator
    {
        public static async Task<ApiResponse> HealthInsuranceRequestModelValidator(HealthInsuranceRequestModel model)
        {
            if (string.IsNullOrEmpty(model.InsuranceCardNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.InsuranceCardNumber)));
            }

            if (string.IsNullOrEmpty(model.RegisteredHospital))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RegisteredHospital)));
            }

            if (model?.ExpirationDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ExpirationDate)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> HealthInsuranceUpdationRequestModelValidator(HealthInsuranceUpdationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.InsuranceCardNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.InsuranceCardNumber)));
            }

            if (string.IsNullOrEmpty(model.RegisteredHospital))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RegisteredHospital)));
            }

            if (model?.ExpirationDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ExpirationDate)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
