using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class NotificationValidator
    {
        public static async Task<ApiResponse> AnnouncementCreationRequestModelValidator(AnnouncementCreationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Title)));
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Description)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
