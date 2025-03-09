using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class VehicleValidator
    {
        public static async Task<ApiResponse> VehicleRequestModelValidator(VehicleRequestModel model)
        {
            if (string.IsNullOrEmpty(model.LicensePlate))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.LicensePlate)));
            }

            if (string.IsNullOrEmpty(model.VehicleType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.VehicleType)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> VehicleUpdationRequestModelValidator(VehicleUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.LicensePlate))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.LicensePlate)));
            }

            if (string.IsNullOrEmpty(model.VehicleType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.VehicleType)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
