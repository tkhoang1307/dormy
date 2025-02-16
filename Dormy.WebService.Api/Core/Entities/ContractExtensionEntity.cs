using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class ContractExtensionEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

        public ContractExtensionStatusEnum Status { get; set; } = ContractExtensionStatusEnum.PENDING;

        public Guid? InvoiceId { get; set; }

        public Guid ApproverId { get; set; }

        public AdminEntity Approver { get; set; } = null!;

        public Guid ContractId { get; set; }

        public ContractEntity Contract { get; set; } = null!;
    }
}
