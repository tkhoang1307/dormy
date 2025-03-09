namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ContractExtensionResponseModel
    {
        public Guid Id { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public ContractResponseModel Contract { get; set; } = null!;
    }
}
