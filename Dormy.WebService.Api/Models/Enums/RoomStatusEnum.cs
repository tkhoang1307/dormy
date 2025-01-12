using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum RoomStatusEnum
    {
        [Description("Available")]
        AVAILABLE,
        [Description("Under maintance")]
        UNDER_MAINTENANCE,
        [Description("Full")]
        FULL,
    }
}
