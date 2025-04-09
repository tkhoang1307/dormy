using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class VehicleHistoryValidator
    {
        public static async Task<ApiResponse> VehicleHistoryRequestModelValidator(VehicleHistoryRequestModel model)
        {
            if (model?.VehicleId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.VehicleId)));
            }
            return new ApiResponse().SetOk();
        }
    }
}
