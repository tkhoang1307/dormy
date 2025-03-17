namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ContractExtensionRequestModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

        public Guid ContractId { get; set; }
    }

    public class ContractExtensionUpdationRequestModel
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}
