using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum InvoiceStatusEnum
    {
        [Description("Draft")]
        DRAFT,
        [Description("Unpaid")]
        UNPAID,
        [Description("Paid")]
        PAID,
        [Description("Overdue")]
        OVERDUE,
        [Description("cancelled")]
        CANCELLED
    }
}
