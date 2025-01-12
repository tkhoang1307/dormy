using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum BedStatusEnum
    {
        [Description("Available")]
        AVAILABLE,
        [Description("Occupied")]
        OCCUPIED,
        [Description("Under maintance")]
        UNDER_MAINTENANCE
    }
}
