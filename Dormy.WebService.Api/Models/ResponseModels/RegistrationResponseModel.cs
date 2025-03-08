using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RegistrationResponseModel
    {
        public Guid RequestId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public Guid? ContractId { get; set; }
        public UserRegisterResponseModel User { get; set; } = null!;
        public WorkplaceRegistrationResponseMode Workplace { get; set; } = null!;
        public RoomTypeRegistrationResponseModel RoomType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HealthInsuranceRegistrationResponseModel HealthInsurance { get; set; } = null!;
        public List<GuardianResponseRegistrationModel>? Guardians { get; set; }
        public List<VehicleResponseRegistrationModel>? Vehicles { get; set; }
    }
}
