namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ContractExtensionRequestModel
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

        //public Guid ContractId { get; set; }
    }

    public class ContractExtensionUpdationRequestModel
    {
        public Guid Id { get; set; }

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
    }
}
