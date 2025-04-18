namespace Dormy.WebService.Api.Models.RequestModels
{
    public class HealthInsuranceRequestModel
    {
        public string InsuranceCardNumber { get; set; } = string.Empty;

        public string RegisteredHospital { get; set; } = string.Empty;

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set => _expirationDate = value.Date;
        }

        public Guid? UserId { get; set; }
    }

    public class HealthInsuranceUpdationRequestModel
    {
        public Guid Id { get; set; }

        public string InsuranceCardNumber { get; set; } = string.Empty;

        public string RegisteredHospital { get; set; } = string.Empty;

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set => _expirationDate = value.Date;
        }

    }
}
