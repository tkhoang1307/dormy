using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IInvoiceUserService
    {
        Task<ApiResponse> CreateInvoiceUsersBatch(Guid roomId);

        Task<ApiResponse> HardDeleteInvoiceUsersBatchByInvoiceId(Guid invoiceId);
    }
}
