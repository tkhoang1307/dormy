using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class BuildingValidator
    {
        public static async Task<ApiResponse> BuildingCreationRequestModelValidator(BuildingCreationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name)));
            }

            if (string.IsNullOrEmpty(model.GenderRestriction))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.GenderRestriction)));
            }

            if (model?.TotalFloors == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.TotalFloors)));
            }

            if (!Enum.TryParse(model.GenderRestriction, out GenderEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.GenderRestriction, nameof(GenderEnum)));
            }

            if (model.TotalFloors <= 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.TotalFloors)));
            }

            if (model.Rooms.Any(room => room.FloorNumber > model.TotalFloors))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, nameof(RoomRequestModel.FloorNumber), nameof(model.TotalFloors)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> BuildingUpdationRequestModelValidator(BuildingUpdationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name)));
            }

            if (string.IsNullOrEmpty(model.GenderRestriction))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.GenderRestriction)));
            }

            if (model?.TotalFloors == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.TotalFloors)));
            }

            if (!Enum.TryParse(model.GenderRestriction, out GenderEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.GenderRestriction, nameof(GenderEnum)));
            }

            if (model.TotalFloors <= 0)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.TotalFloors)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
