
namespace Dormy.WebService.Api.Models.RequestModels
{
    public class SettingRequestModel
    {
        public string KeyName { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;

        public bool? IsApplied { get; set; }
    }

    public class SettingUpdateValueRequestModel
    {
        public string KeyName { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;
    }

    public class SettingTurnOnOffRequestModel
    {
        public string KeyName { get; set; } = string.Empty;

        public bool IsApplied { get; set; }
    }
}
