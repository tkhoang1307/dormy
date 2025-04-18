using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ContractRequestModel
    {
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

        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }
    }

    public class ContractUpdationRequestModel
    {
        public Guid ContractId { get; set; }

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
