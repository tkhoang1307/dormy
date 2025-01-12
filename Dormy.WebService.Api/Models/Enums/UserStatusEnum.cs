using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum UserStatusEnum
    {
        [Description("Active")]
        ACTIVE,
        [Description("Suspended")]
        SUSPENDED,
        [Description("Inactive")]
        INACTIVE
    }
}
