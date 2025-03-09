using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class RoomTypeValidator
    {
        public static async Task<ApiResponse> RoomTypeRequestModelValidator(RoomTypeRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RoomTypeName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomTypeName)));
            }

            if (model?.Capacity == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Capacity)));
            }

            if (model?.Price == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Price)));
            }

            if (model.Capacity <= 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.Capacity)));
            }

            if (model.Price < 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.Price)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> RoomTypeUpdateRequestModelValidator(RoomTypeUpdateRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.RoomTypeName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomTypeName)));
            }

            if (model?.Capacity == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Capacity)));
            }

            if (model?.Price == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Price)));
            }

            if (model.Capacity <= 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.Capacity)));
            }

            if (model.Price < 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.Price)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
