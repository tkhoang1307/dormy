using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ContractResponseModel
    {
        public Guid Id { get; set; }

        public DateTime SubmissionDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ContractStatusEnum Status { get; set; }

        public int NumberExtension { get; set; } = 0;

        public Guid? ApproverId { get; set; }

        public string? ApproverName { get; set; }

        public Guid RoomId { get; set; }
        public RoomResponseModel Room { get; set; } = null!;
    }
}
