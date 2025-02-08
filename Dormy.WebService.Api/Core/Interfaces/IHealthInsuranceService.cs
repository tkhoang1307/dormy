using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IHealthInsuranceService
    {
        Task<ApiResponse> AddHealthInsurance(HealthInsuranceRequestModel model);
        Task<ApiResponse> UpdateHealthInsurance(HealthInsuranceUpdationRequestModel model);
        Task<ApiResponse> GetDetailHealthInsurance(Guid id);
    }
}
