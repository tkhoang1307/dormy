using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ContractValidator
    {
        public static async Task<ApiResponse> ApproveOrRejectContractRequestModelValidator(ApproveOrRejectContractRequestModel model)
        {
            if (model?.IsAccepted == null)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.IsAccepted)));
            }

            return new ApiResponse().SetOk();
        }
    }
}
