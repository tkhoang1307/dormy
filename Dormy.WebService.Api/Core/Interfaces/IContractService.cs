using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IContractService
    {
        Task<ApiResponse> Register(RegisterRequestModel model);
        Task<ApiResponse> UpdateRegistration(Guid requestId, RegisterUpdateRequestModel model);
        Task<ApiResponse> GetRegistrationByRequestId(Guid requestId, Guid? userId);
        Task<ApiResponse> GetRegistrationBatchByRequestIds(List<Guid> requestIds, Guid? userId);
        Task<ApiResponse> UpdateContractStatus(Guid id, ContractStatusEnum status);
        Task<ApiResponse> GetSingleContract(Guid id);
        Task<ApiResponse> GetContractBatch(GetBatchRequestModel model);
    }
}
