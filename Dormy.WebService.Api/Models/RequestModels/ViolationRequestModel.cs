namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ViolationRequestModel
    {
        public string Description { get; set; } = string.Empty;

        public DateTime ViolationDate { get; set; } = DateTime.Now;

        public decimal Penalty { get; set; }

        public Guid UserId { get; set; }
    }
}
