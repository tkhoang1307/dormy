namespace Dormy.WebService.Api.Models.RequestModels
{
    public class OvernightAbsentRequestModel
    {
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public DateTime EndDateTime { get; set; } = DateTime.Now.AddDays(1);

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
