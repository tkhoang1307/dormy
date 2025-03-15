using Dormy.WebService.Api.Models.Enums;
using Newtonsoft.Json.Linq;

namespace Dormy.WebService.Api.Core.Entities
{
    public class InvoiceEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string InvoiceName { get; set; } = string.Empty;

        public DateTime DueDate { get; set; } = DateTime.Now;

        public decimal AmountBeforePromotion { get; set; }

        public decimal AmountAfterPromotion { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public InvoiceTypeEnum Type { get; set; }

        public InvoiceStatusEnum Status { get; set; } = InvoiceStatusEnum.DRAFT;

        public Guid? ContractId { get; set; }

        public Guid RoomId { get; set; }

        public RoomEntity Room { get; set; } = null!;

        public List<InvoiceItemEntity>? InvoiceItems { get; set; }

        public List<InvoiceUserEntity>? InvoiceUsers { get; set; }
    }
}
