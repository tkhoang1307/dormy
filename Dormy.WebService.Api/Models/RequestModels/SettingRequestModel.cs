
namespace Dormy.WebService.Api.Models.RequestModels
{
    public class SettingRequestModel
    {
        public string Name { get; set; } = string.Empty;

        public bool ParameterBool { get; set; }

        public DateTime ParameterDate { get; set; } = DateTime.Now;
    }

    public class SettingUpdateRequestModel : SettingRequestModel
    {
        public Guid Id { get; set; }
    }
}
