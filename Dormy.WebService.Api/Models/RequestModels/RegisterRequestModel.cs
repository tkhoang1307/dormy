namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public UserRequestModel User { get; set; } = null!;
        public Guid? WorkplaceId { get; set; }
        public Guid RoomId { get; set; }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value.Date;
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value.Date;
        }
        public HealthInsuranceRequestModel HealthInsurance { get; set; } = null!;
        public List<GuardianRequestModel>? Guardians { get; set; } = new List<GuardianRequestModel>();
        public List<VehicleRequestModel>? Vehicles { get; set; } = new List<VehicleRequestModel>();
    }
}
