using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ParkingSpotValidator
    {
        public static async Task<ApiResponse> ParkingSpotRequestModelValidator(ParkingSpotRequestModel model)
        {
            if (string.IsNullOrEmpty(model.ParkingSpotName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotName)));
            }

            if (model?.CapacitySpots == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.CapacitySpots)));
            }

            if (model.CapacitySpots <= 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.CapacitySpots)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ParkingSpotUpdationRequestModelValidator(ParkingSpotUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.ParkingSpotName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotName)));
            }

            if (model?.CapacitySpots == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.CapacitySpots)));
            }

            if (model.CapacitySpots <= 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.CapacitySpots)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ParkingSpotUpdateStatusRequestModelValidator(ParkingSpotUpdateStatusRequestModel model)
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

            if (!Enum.TryParse(model.Status, out ParkingSpotStatusEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Status, nameof(ParkingSpotStatusEnum)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
