namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RequestResponseModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public Guid? ApproverId { get; set; }
        public string ApproverName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid? RoomId { get; set; }
        public RoomResponseModel? Room { get; set; }
        public ContractResponseModel? Contract { get; set; }
    }
}
