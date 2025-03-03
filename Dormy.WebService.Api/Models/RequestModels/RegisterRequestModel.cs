namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public UserRegisterRequestModel User { get; set; } // Changed from private to public
        public Guid? WorkplaceId { get; set; }
        public Guid RoomTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HealthInsuranceRequestModel HealthInsurance { get; set; }
        public GuardianRequestModel? Guardian { get; set; }
        public VehicleRequestModel? Vehicle { get; set; }
    }
}
