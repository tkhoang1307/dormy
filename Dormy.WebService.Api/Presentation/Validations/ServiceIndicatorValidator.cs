using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ServiceIndicatorValidator
    {
        public static async Task<ApiResponse> ServiceIndicatorRequestModelValidator(ServiceIndicatorRequestModel model)
        {
            if (model?.RoomServiceId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceId)));
            }

            if (model?.RoomId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId)));
            }

            if (model?.Month == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month)));
            }

            if (model?.Year == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year)));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month)));
            }

            if (model?.NewIndicator == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NewIndicator)));
            }

            if (model.NewIndicator < 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.NewIndicator)));
            }

            if (model?.OldIndicator != null)
            {
                if (model.OldIndicator < 0)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.OldIndicator)));
                }

                if (model.OldIndicator > model.NewIndicator)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(model.OldIndicator), nameof(model.NewIndicator)));
                }
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ServiceIndicatorCreationBatchRequestModelValidator(ServiceIndicatorCreationBatchRequestModel model)
        {
            if (model?.RoomId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId)));
            }

            if (model?.Month == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month)));
            }

            if (model?.Year == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year)));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month)));
            }

            foreach (var roomServiceIndicator in model.RoomServiceIndicators)
            {
                if (roomServiceIndicator?.RoomServiceId == null)
                {
                    return new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(roomServiceIndicator.RoomServiceId)));
                }

                if (roomServiceIndicator?.NewIndicator == null)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(roomServiceIndicator.NewIndicator)));
                }

                if (roomServiceIndicator.NewIndicator < 0)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(roomServiceIndicator.NewIndicator)));
                }

                if (roomServiceIndicator?.OldIndicator != null && roomServiceIndicator.OldIndicator > roomServiceIndicator.NewIndicator)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(roomServiceIndicator.OldIndicator), nameof(roomServiceIndicator.NewIndicator)));
                }
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ServiceIndicatorUpdationRequestModelValidator(ServiceIndicatorUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (model?.RoomServiceId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomServiceId)));
            }

            if (model?.RoomId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId)));
            }

            if (model?.Month == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Month)));
            }

            if (model?.Year == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Year)));
            }

            if (model.Month <= 0 || model.Month >= 13)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.InvalidMonth, nameof(model.Month)));
            }

            if (model?.NewIndicator == null)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NewIndicator)));
            }

            if (model.NewIndicator < 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.NewIndicator)));
            }

            if (model?.OldIndicator != null)
            {
                if (model.OldIndicator < 0)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.OldIndicator)));
                }

                if (model.OldIndicator > model.NewIndicator)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(model.OldIndicator), nameof(model.NewIndicator)));
                }
            }

            return new ApiResponse().SetOk();
        }
    }
}
