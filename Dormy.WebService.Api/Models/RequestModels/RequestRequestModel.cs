namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RequestRequestModel
    {
        public string Description { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public Guid? RoomId { get; set; }
    }

    public class RequestUpdationRequestModel : RequestRequestModel
    {
        public Guid Id { get; set; }
    }

    public class RequestApproveOrRejectRequestModel
    {
        public Guid Id { get; set; }

        public bool IsApproved { get; set; }
    }
}
