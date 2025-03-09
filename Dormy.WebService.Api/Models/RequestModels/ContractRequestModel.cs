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
}
