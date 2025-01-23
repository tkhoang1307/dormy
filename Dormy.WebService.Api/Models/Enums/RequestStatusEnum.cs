using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum RequestStatusEnum
    {
        [Description("Submitted")]
        SUBMITTED,
        [Description("Approved")]
        APPROVED,
        [Description("Rejected")]
        REJECTED,
        [Description("Cancelled")]
        CANCELLED,
    }
}
