namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class SettingResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string KeyName { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;

        public bool IsApplied { get; set; }
    }
}
