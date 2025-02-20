using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum InvoiceTypeEnum
    {
        [Description("Room serivce monthly")]
        ROOM_SERVICE_MONTHLY,
        [Description("Payment contract")]
        PAYMENT_CONTRACT
    }
}
