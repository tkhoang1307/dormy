namespace Dormy.WebService.Api.Core.Entities
{
    public class HealthInsuranceEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string InsuranceCardNumber { get; set; } = string.Empty;

        public string RegisteredHospital { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; } = DateTime.Now;

        public UserEntity User { get; set; } = null!;
    }
}
