using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class SettingValidator
    {
        public static async Task<ApiResponse> SettingRequestModelValidator(SettingRequestModel model)
        {
            if (string.IsNullOrEmpty(model.KeyName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.KeyName)));
            }

            if (string.IsNullOrEmpty(model.DataType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DataType)));
            }

            if (!Enum.TryParse(model.DataType, out SettingDataTypeEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.DataType, nameof(SettingDataTypeEnum)));
            }

            if (model.Value != SettingDataTypeEnum.BOOL.ToString() && string.IsNullOrEmpty(model.Value))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Value)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> SettingUpdateValueRequestModelValidator(SettingUpdateValueRequestModel model)
        {
            if (string.IsNullOrEmpty(model.KeyName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.KeyName)));
            }

            if (string.IsNullOrEmpty(model.DataType))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DataType)));
            }

            if (!Enum.TryParse(model.DataType, out SettingDataTypeEnum result))
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.DataType, nameof(SettingDataTypeEnum)));
            }

            if (model.Value != SettingDataTypeEnum.BOOL.ToString() && string.IsNullOrEmpty(model.Value))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Value)));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> SettingTurnOnOffRequestModelValidator(SettingTurnOnOffRequestModel model)
        {
            if (string.IsNullOrEmpty(model.KeyName))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.KeyName)));
            }

            if (model?.IsApplied == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsApplied)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
