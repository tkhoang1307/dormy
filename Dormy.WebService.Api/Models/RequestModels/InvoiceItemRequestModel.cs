using Newtonsoft.Json.Linq;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class InvoiceItemRequestModel
    {
        public Guid RoomServiceId { get; set; }

        public decimal Quantity { get; set; }

        public decimal? OldIndicator { get; set; }

        public decimal? NewIndicator { get; set; }
    }

    public class InvoiceItemMapperRequestModel
    {
        public string RoomServiceName { get; set; } = string.Empty;

        public Guid RoomServiceId { get; set; }

        public decimal Cost { get; set; }

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        public JObject? Metadata { get; set; } = null!;
    }
}
