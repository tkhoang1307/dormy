using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class UserValidator
    {
        public static async Task<ApiResponse> UserRequestModelValidator(UserRequestModel request)
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

            if (string.IsNullOrEmpty(request.NationalIdNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.NationalIdNumber)));
            }

            if (!Enum.TryParse(request.Gender, out GenderEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, request.Gender, nameof(GenderEnum)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> LoginRequestModelValidator(LoginRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Username)));
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Password)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> ChangePasswordRequestModelValidator(ChangePasswordRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.OldPassword))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.OldPassword)));
            }

            if (string.IsNullOrEmpty(model.NewPassword))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NewPassword)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> UserUpdateRequestModelValidator(UserUpdateRequestModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.FirstName)));
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.LastName)));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Email)));
            }

            if (model?.DateOfBirth == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DateOfBirth)));
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.PhoneNumber)));
            }

            if (string.IsNullOrEmpty(model.NationalIdNumber))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NationalIdNumber)));
            }

            if (!Enum.TryParse(model.Gender, out GenderEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Gender, nameof(GenderEnum)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
