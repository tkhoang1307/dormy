using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class RegistrationValidator
    {
        public static async Task<ApiResponse> RegisterRequestModelValidator(RegisterRequestModel model)
        {
            if (model.User != null)
            {
                var userModelValidator = await UserValidator.UserRequestModelValidator(model.User);
                if (!userModelValidator.IsSuccess)
                {
                    return userModelValidator;
                }
            }

            if (model.HealthInsurance != null)
            {
                var healthInsuranceModelValidator = await HealthInsuranceValidator.HealthInsuranceRequestModelValidator(model.HealthInsurance);
                if (!healthInsuranceModelValidator.IsSuccess)
                {
                    return healthInsuranceModelValidator;
                }
            }

            if (model.HealthInsurance != null)
            {
                var healthInsuranceModelValidator = await HealthInsuranceValidator.HealthInsuranceRequestModelValidator(model.HealthInsurance);
                if (!healthInsuranceModelValidator.IsSuccess)
                {
                    return healthInsuranceModelValidator;
                }
            }

            foreach(var guardian in model.Guardians)
            {
                var guardianModelValidator = await GuardianValidator.GuardianRequestModelValidator(guardian);
                if (!guardianModelValidator.IsSuccess)
                {
                    return guardianModelValidator;
                }
            }

            foreach(var vehicle in model.Vehicles)
            {
                var vehicleModelValidator = await VehicleValidator.VehicleRequestModelValidator(vehicle);
                if (!vehicleModelValidator.IsSuccess)
                {
                    return vehicleModelValidator;
                }
            }

            if (model?.RoomId == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomId)));
            }

            if (model?.StartDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.StartDate)));
            }

            if (model?.EndDate == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.EndDate)));
            }

            if (model.StartDate.Date >= model.EndDate.Date)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.StartDateMustBeLessThanEndDate));
            }

            return new ApiResponse().SetOk();
        }
    }
}
