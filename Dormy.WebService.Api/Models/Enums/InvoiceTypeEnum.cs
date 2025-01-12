using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum InvoiceTypeEnum
    {
        [Description("Monthly")]
        MONTHLY,
        [Description("Quarterly")]
        QUARTERLY,
        [Description("Yearly")]
        YEARLY,
        [Description("Payment contract")]
        PAYMENT_CONTRACT
    }
}
