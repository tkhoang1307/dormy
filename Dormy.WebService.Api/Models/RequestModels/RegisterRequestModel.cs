namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public UserRequestModel User { get; set; } = null!;
        public Guid? WorkplaceId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HealthInsuranceRequestModel HealthInsurance { get; set; } = null!;
        public List<GuardianRequestModel>? Guardians { get; set; } = new List<GuardianRequestModel>();
        public List<VehicleRequestModel>? Vehicles { get; set; } = new List<VehicleRequestModel>();
    }
}
