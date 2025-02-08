namespace Dormy.WebService.Api.Models.RequestModels
{
    public class HealthInsuranceRequestModel
    {
        public string InsuranceCardNumber { get; set; } = string.Empty;

        public string RegisteredHospital { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; } = DateTime.Now;
    }

    public class HealthInsuranceUpdationRequestModel : HealthInsuranceRequestModel
    {
        public Guid Id { get; set; }
    }
}
