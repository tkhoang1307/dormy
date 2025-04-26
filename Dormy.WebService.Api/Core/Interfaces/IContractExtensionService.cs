using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IContractExtensionService
    {
        Task<ApiResponse> CreateContractExtension(ContractExtensionRequestModel model);
        Task<ApiResponse> GetSingleContractExtensionById(Guid id);
        Task<ApiResponse> GetContractExtensionBatch(GetBatchRequestModel model);
        Task<ApiResponse> UpdateContractExtensionStatus(Guid id, ContractExtensionStatusEnum status);
        Task<ApiResponse> UpdateContractExtension(ContractExtensionUpdationRequestModel model);

        Task<ApiResponse> GetRegistrationAccommodationBatch();
    }
}
