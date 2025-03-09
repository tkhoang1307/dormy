using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ParkingRequestValidator
    {
        public static async Task<ApiResponse> ParkingRequestModelValidator(ParkingRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Description)));
            }

            if (model?.ParkingSpotId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotId)));
            }

            if (model?.VehicleId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.VehicleId)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> UpdateParkingRequestModelValidator(UpdateParkingRequestModel model)
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

            if (model?.ParkingSpotId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotId)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ApproveOrRejectParkingRequestModelValidator(ApproveOrRejectParkingRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (model?.IsAccepted == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsAccepted)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
