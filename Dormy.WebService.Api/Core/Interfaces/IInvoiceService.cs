using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IInvoiceService
    {
        Task<ApiResponse> CreateNewInvoice(InvoiceRequestModel model);
        Task<ApiResponse> GetInvoiceById(Guid id);
        Task<ApiResponse> GetInvoiceBatch(GetBatchInvoiceRequestModel model);
        Task<ApiResponse> UpdateInvoiceStatus(InvoiceStatusUpdationRequestModel model);
        Task<ApiResponse> UpdateInvoice(InvoiceUpdationRequestModel model);
        Task<ApiResponse> HardDeleteInvoiceById(Guid id);
        Task<ApiResponse> GetInitialInvoiceCreation(GetInitialInvoiceCreationRequestModel model);

        Task<ApiResponse> GetInitialInvoiceEdit(Guid id);
        Task<ApiResponse> GetRoomsForInitialInvoiceCreation();
    }
}
