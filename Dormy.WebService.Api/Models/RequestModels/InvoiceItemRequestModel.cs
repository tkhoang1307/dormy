namespace Dormy.WebService.Api.Models.RequestModels
{
    public class InvoiceItemRequestModel
    {
        public Guid RoomServiceId { get; set; }

        public decimal Quantity { get; set; }

        public string Metadata { get; set; } = string.Empty;
    }

    public class InvoiceItemMapperRequestModel
    {
        public string RoomServiceName { get; set; } = string.Empty;

        public Guid RoomServiceId { get; set; }

        public decimal Cost { get; set; }

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        public string Metadata { get; set; } = string.Empty;
    }
}
