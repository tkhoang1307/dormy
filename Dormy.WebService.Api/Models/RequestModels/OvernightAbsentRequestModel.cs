namespace Dormy.WebService.Api.Models.RequestModels
{
    public class OvernightAbsentRequestModel
    {
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public DateTime EndDateTime { get; set; } = DateTime.Now.AddDays(1);

        public string Reason { get; set; } = string.Empty;
    }
}
