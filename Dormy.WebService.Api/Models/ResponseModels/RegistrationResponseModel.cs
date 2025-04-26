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

    public class RegistrationAccommodationResponseModel
    {
        public Guid ContractExtensionId { get; set; }
        public int OrderNo { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserFullname { get; set; } = string.Empty;
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public Guid BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public RegistrationAccommodationContractResponseModel ContractInformation { get; set; } = null!;
    }

    public class RegistrationAccommodationContractResponseModel
    {
        public Guid ContractId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int NumberExtension { get; set; } = 0;
    }
}
