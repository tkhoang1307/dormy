namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ContractResponseModel
    {
        public Guid Id { get; set; }

        public DateTime SubmissionDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public int NumberExtension { get; set; } = 0;

        public Guid? ApproverId { get; set; }

        public string? ApproverName { get; set; }

        public RoomResponseModel Room { get; set; } = null!;

        public UserResponseModel User { get; set; } = null!;

        public WorkplaceResponseModel? Workplace { get; set; }

        public HealthInsuranceResponseModel? HealthInsurance { get; set; }

        public List<GuardianResponseModel> Guardians { get; set; } = [];

        public List<VehicleResponseModel> Vehicles { get; set; } = [];
    }
}
