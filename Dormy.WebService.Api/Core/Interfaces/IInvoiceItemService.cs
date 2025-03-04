using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IInvoiceItemService
    {
        Task<ApiResponse> CreateInvoiceItemsBatch(List<InvoiceItemRequestModel> models);

        Task<ApiResponse> HardDeleteInvoiceItemsBatchByInvoiceId(Guid invoiceId);
    }
}
