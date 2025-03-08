using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class InvoiceRequestModel
    {
        public DateTime DueDate { get; set; } = DateTime.Now;

        public int? Month { get; set; }

        public int? Year { get; set; }

        // InvoiceTypeEnum
        public string Type { get; set; } = string.Empty;

        public Guid RoomId { get; set; }

        public List<InvoiceItemRequestModel> InvoiceItems { get; set; } = null!;
    }

    public class InvoiceUpdationRequestModel : InvoiceRequestModel
    {
        public Guid Id { get; set; }
    }

    public class InvoiceStatusUpdationRequestModel
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = string.Empty;
    }

    public class InvoiceMapperRequestModel
    {
        public string InvoiceName { get; set; } = string.Empty;

        public DateTime DueDate { get; set; } = DateTime.Now;

        public decimal AmountBeforePromotion { get; set; }

        public decimal AmountAfterPromotion { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public Guid RoomId { get; set; }

        public List<InvoiceItemMapperRequestModel> InvoiceItems { get; set; } = null!;

        public List<InvoiceUserMapperModel> InvoiceUsers { get; set; } = null!;
    }

    public class InvoiceUserMapperModel
    {
        public Guid InvoiceId { get; set; }

        public Guid UserId { get; set; }

    }

    public class GetInitialInvoiceCreationRequestModel
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public Guid RoomId { get; set; }
    }
}
