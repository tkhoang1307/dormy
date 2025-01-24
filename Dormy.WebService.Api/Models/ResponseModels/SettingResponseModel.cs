namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class SettingResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public bool ParameterBool { get; set; }

        public DateTime ParameterDate { get; set; } = DateTime.Now;
    }
}
