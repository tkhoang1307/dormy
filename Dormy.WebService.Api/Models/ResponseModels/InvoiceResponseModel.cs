using Newtonsoft.Json.Linq;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class InvoiceBatchResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string InvoiceName { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public decimal AmountBeforePromotion { get; set; }

        public decimal AmountAfterPromotion { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public Guid? ContractId { get; set; }

        public Guid RoomId { get; set; }

        public string RoomName { get; set; } = string.Empty;
    }

    public class InvoiceResponseModel : InvoiceBatchResponseModel
    {
        public List<InvoiceItemResponseModel>? InvoiceItems { get; set; }

        public List<InvoiceUserResponseModel>? InvoiceUsers { get; set; }
    }

    public class InvoiceUserResponseModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;
    }

    public class GetInitialInvoiceCreationResponseModel
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public Guid RoomId { get; set; }

        public List<GetInitialInvoiceItemCreationResponseModel> RoomServices { get; set; } = [];
    }

    public class GetInitialInvoiceItemCreationResponseModel
    {
        public Guid RoomServiceId { get; set; }

        public string RoomServiceName { get; set; } = string.Empty;

        public string RoomServiceType { get; set; } = string.Empty;

        public bool IsServiceIndicatorUsed { get; set; }

        public decimal? CurrentIndicator { get; set; }
    }
}
