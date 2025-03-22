using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IDormyJobsService
    {
        Task<ApiResponse> ContractJob();

        Task<ApiResponse> ContractExtensionJob();

        Task<ApiResponse> ỊnvoiceJob();
    }
}
