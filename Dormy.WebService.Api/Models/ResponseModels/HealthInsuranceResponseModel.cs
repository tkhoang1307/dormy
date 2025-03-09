namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class HealthInsuranceResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string InsuranceCardNumber { get; set; } = string.Empty;

        public string RegisteredHospital { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }

        public UserResponseModel User { get; set; } = null!;
    }
}
