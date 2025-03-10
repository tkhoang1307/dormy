using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ContractRequestModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }
    }

    public class ContractUpdationRequestModel
    {
        public Guid ContractId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid RoomId { get; set; }
    }

    public class ApproveOrRejectContractRequestModel
    {
        public bool IsAccepted { get; set; }
    }

    public class ContractUpdationStatusRequestModel
    {
        public Guid Id { get; set; }
        public ContractStatusEnum Status { get; set; }
    }
}
