using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum ContractExtensionStatusEnum
    {
        [Description("Pending")]
        PENDING,
        [Description("Waiting payment")]
        WAITING_PAYMENT,
        [Description("Active")]
        ACTIVE,
        [Description("Expired")]
        EXPIRED,
        [Description("Terminated")]
        TERMINATED,
        [Description("Rejected")]
        REJECTED,
    }
}
