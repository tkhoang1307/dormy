using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum RoomServiceTypeEnum
    {
        [Description("Electricity /*-*/ Điện")]
        ELECTRICITY,

        [Description("Water /*-*/ Nước")]
        WATER,

        [Description("Parking fee /*-*/ Phí đỗ xe")]
        PARKING_FEE,

        [Description("Refrigerator /*-*/ Tủ lạnh")]
        REFRIGERATOR,

        [Description("Wardrobe /*-*/ Tủ quần áo")]
        WARDROBE,

        [Description("Wifi fee /*-*/ Phí wifi")]
        WIFI_FEE,

        [Description("Air conditioning /*-*/ Điều hòa")]
        AIR_CONDITIONING,

        [Description("Garbage fee /*-*/ Phí rác")]
        GARBAGE_FEE,

        [Description("Washing machine /*-*/ Máy giặt")]
        WASHING_MACHINE,

        [Description("Other services /*-*/ Dịch vụ khác")]
        OTHER_SERVICES
    }
}
