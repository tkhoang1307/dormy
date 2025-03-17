namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RegistrationResponseModel
    {
        public Guid ContractId { get; set; }

        public Guid UserId { get; set; }

        public Guid HealthInsuranceId { get; set; }

        public List<Guid>? GuardianIds { get; set; } = [];

        public List<Guid>? VehicleIds { get; set; } = [];
    }
}
