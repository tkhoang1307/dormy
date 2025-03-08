namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public UserRegisterRequestModel User { get; set; } = null!;
        public Guid? WorkplaceId { get; set; }
        public Guid RoomTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HealthInsuranceRequestModel HealthInsurance { get; set; } = null!;
        public List<GuardianRequestModel>? Guardians { get; set; }
        public List<VehicleRequestModel>? Vehicles { get; set; }
    }

    public class RegisterUpdateRequestModel
    {
        public UserRegisterRequestModel User { get; set; } = null!;
        public Guid? WorkplaceId { get; set; }
        public Guid RoomTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HealthInsuranceUpdationRequestModel HealthInsurance { get; set; } = null!;
        public List<GuardianRequestModel>? Guardians { get; set; }
        public List<VehicleRequestModel>? Vehicles { get; set; }
    }
}
