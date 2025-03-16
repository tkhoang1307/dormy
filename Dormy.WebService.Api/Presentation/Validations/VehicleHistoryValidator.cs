using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class VehicleHistoryValidator
    {
        public static async Task<ApiResponse> VehicleHistoryRequestModelValidator(VehicleHistoryRequestModel model)
        {
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

            if (model?.IsIn == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsIn)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
