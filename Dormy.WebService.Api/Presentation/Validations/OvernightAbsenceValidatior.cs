using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class OvernightAbsenceValidatior
    {
        public static async Task<ApiResponse> OvernightAbsentRequestModelValidator(OvernightAbsentRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Reason))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Reason)));
            }

            if (model?.StartDateTime == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.StartDateTime)));
            }

            if (model?.EndDateTime == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.EndDateTime)));
            }

            // Validate startDate and endDate
            if (model.StartDateTime.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.StartDateTime)));
            }
            if (model.EndDateTime.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.EndDateTime)));
            }
            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(model.StartDateTime, model.EndDateTime, true))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.StartDateMustBeLessThanOrEqualToEndDate));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> OvernightAbsentUpdationRequestModelValidator(OvernightAbsentUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (string.IsNullOrEmpty(model.Reason))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Reason)));
            }

            if (model?.StartDateTime == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.StartDateTime)));
            }

            if (model?.EndDateTime == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.EndDateTime)));
            }

            // Validate startDate and endDate
            if (model.StartDateTime.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.StartDateTime)));
            }
            if (model.EndDateTime.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.EndDateTime)));
            }
            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(model.StartDateTime, model.EndDateTime, true))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.StartDateMustBeLessThanOrEqualToEndDate));
            }

            return new ApiResponse().SetOk();
        }

        public static async Task<ApiResponse> OvernightAbsentApproveOrRejectRequestModelValidator(OvernightAbsentApproveOrRejectRequestModel model)
        {
            if (model?.Id == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id)));
            }

            if (model?.IsApproved == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsApproved)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
