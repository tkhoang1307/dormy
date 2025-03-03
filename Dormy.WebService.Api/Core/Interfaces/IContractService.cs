using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IContractService
    {
        Task<ApiResponse> Register(RegisterRequestModel model);
    }
}
