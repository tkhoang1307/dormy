namespace Dormy.WebService.Api.Models.RequestModels
{
    public class OvernightAbsentRequestModel
    {
        private DateTime _startDateTime;
        public DateTime StartDateTime
        {
            get => _startDateTime;
            set => _startDateTime = value.Date;
        }

        private DateTime _endDateTime = DateTime.Now.AddDays(1);
        public DateTime EndDateTime
        {
            get => _endDateTime;
            set => _endDateTime = value.Date;
        }

        public string Reason { get; set; } = string.Empty;
    }

    public class OvernightAbsentUpdationRequestModel : OvernightAbsentRequestModel
    {
        public Guid Id { get; set; }
    }

    public class OvernightAbsentApproveOrRejectRequestModel
    {
        public bool IsApproved { get; set; }
    }
}
