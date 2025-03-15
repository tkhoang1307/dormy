using Newtonsoft.Json.Linq;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class InvoiceItemResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string RoomServiceName { get; set; } = string.Empty;

        public Guid? RoomServiceId { get; set; }

        public decimal Cost { get; set; }

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        public decimal? OldIndicator { get; set; }

        public decimal? NewIndicator { get; set; }
    }
}
