namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ViolationRequestModel
    {
        public string Description { get; set; } = string.Empty;

        private DateTime _violationDate;
        public DateTime ViolationDate
        {
            get => _violationDate;
            set => _violationDate = value.Date;
        }

        public decimal Penalty { get; set; }

        public Guid UserId { get; set; }
    }

    public class ViolationUpdationRequestModel : ViolationRequestModel
    {
        public Guid Id { get; set; }

    }
}
