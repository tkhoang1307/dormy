using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class ContractEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

        public ContractStatusEnum Status { get; set; } = ContractStatusEnum.PENDING;

        public int NumberExtension { get; set; } = 0;

        public Guid? InvoiceId { get; set; }

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;

        public Guid ApproverId { get; set; }

        public AdminEntity Approver { get; set; } = null!;

        public Guid RoomId { get; set; }

        public RoomEntity Room { get; set; } = null!;
    }
}
