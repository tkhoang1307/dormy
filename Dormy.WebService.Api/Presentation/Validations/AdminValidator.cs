using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class AdminValidator
    {
        public static async Task<ApiResponse> AdminRequestModelValidator(AdminRequestModel request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.FirstName)));
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.LastName)));
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.Email)));
            }

            if (string.IsNullOrEmpty(request.UserName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.UserName)));
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.Password)));
            }

            if (request?.DateOfBirth == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.DateOfBirth)));
            }

            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.PhoneNumber)));
            }

            if (!Enum.TryParse(request.Gender, out GenderEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, request.Gender, nameof(GenderEnum)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
