namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class HealthInsuranceResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string InsuranceCardNumber { get; set; } = string.Empty;

        public string RegisteredHospital { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; } = DateTime.Now;

        public UserResponseModel User { get; set; } = null!;
    }
}
