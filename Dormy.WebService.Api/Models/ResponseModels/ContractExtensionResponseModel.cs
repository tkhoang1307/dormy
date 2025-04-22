namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ContractExtensionResponseModel
    {
        public Guid Id { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? InvoiceId { get; set; }
        public Guid? ApproverId { get; set; }
        public string ApproverFullName { get; set; } = string.Empty;
        public ContractResponseModel Contract { get; set; } = null!;
    }

    public class ContractForContractExtensionResponseModel
    {
        public Guid Id { get; set; }

        public DateTime SubmissionDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public int NumberExtension { get; set; } = 0;

        public Guid UserId { get; set; }

        public string UserFullname { get; set; } = string.Empty;

        public Guid RoomId { get; set; }

        public int RoomNumber { get; set; }

        public Guid RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public Guid BuildingId { get; set; }

        public string BuildingName { get; set; } = string.Empty;

        public Guid? WorkplaceId { get; set; }

        public string WorkplaceName { get; set; } = string.Empty;

        public string InsuranceCardNumber { get; set; } = string.Empty;

        public string RegisteredHospital { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }
    }
}
