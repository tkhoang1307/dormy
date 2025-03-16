using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.IdentityModel.Tokens;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class RoomValidator
    {
        public static async Task<ApiResponse> RoomCreationRequestModelValidator(RoomCreationRequestModel room)
        {
            if (room?.FloorNumber == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.FloorNumber)));
            }

            if (room?.RoomTypeId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.RoomTypeId)));
            }

            if (room?.TotalRoomsWantToCreate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.TotalRoomsWantToCreate)));
            }

            if (room.FloorNumber < 0)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(room.FloorNumber)));
            }

            if (room.TotalRoomsWantToCreate <= 0)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(room.TotalRoomsWantToCreate)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> RoomUpdateRequestModelValidator(RoomUpdateRequestModel room)
        {
            if (room?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.Id)));
            }

            if (room?.FloorNumber == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.FloorNumber)));
            }

            if (room?.RoomTypeId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(room.RoomTypeId)));
            }

            if (room.FloorNumber < 0)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(room.FloorNumber)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> RoomUpdateStatusRequestModelValidator(RoomUpdateStatusRequestModel model)
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

            if (!Enum.TryParse(model.Status, out RoomStatusEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Status, nameof(RoomStatusEnum)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
