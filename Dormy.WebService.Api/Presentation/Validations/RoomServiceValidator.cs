using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class RoomServiceValidator
    {
        public static async Task<ApiResponse> RoomServiceRequestModelValidator(RoomServiceRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RoomServiceName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceName)));
            }

            if (string.IsNullOrEmpty(model.Unit))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Unit)));
            }

            if (model?.Cost == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Cost)));
            }

            if (model.Cost < 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.Cost)));
            }

            if (string.IsNullOrEmpty(model.RoomServiceType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceType)));
            }

            if (!Enum.TryParse(model.RoomServiceType, out RoomServiceTypeEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.RoomServiceType, nameof(RoomServiceTypeEnum)));
            }

            if (model?.IsServiceIndicatorUsed == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsServiceIndicatorUsed)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> RoomServiceUpdateRequestModelValidator(RoomServiceUpdateRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RoomServiceName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceName)));
            }

            if (string.IsNullOrEmpty(model.Unit))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Unit)));
            }

            if (model?.Cost == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Cost)));
            }

            if (model.Cost < 0)
            {
                return new ApiResponse().SetPreconditionFailed(message: string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.Cost)));
            }

            if (string.IsNullOrEmpty(model.RoomServiceType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceType)));
            }

            if (!Enum.TryParse(model.RoomServiceType, out RoomServiceTypeEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.RoomServiceType, nameof(RoomServiceTypeEnum)));
            }

            if (model?.IsServiceIndicatorUsed == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsServiceIndicatorUsed)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
