using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum SettingDataTypeEnum
    {
        [Description("int")]
        INT,
        [Description("decimal")]
        DECIMAL,
        [Description("bool")]
        BOOL,
        [Description("char")]
        CHAR,
        [Description("Guid")]
        GUID,
        [Description("DateTime")]
        DATETIME,
        [Description("dynamic")]
        DYNAMIC,
        [Description("object")]
        OBJECT,
    }
}
