namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RequestRequestModel
    {
        public string Description { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public Guid? RoomId { get; set; }
    }
}
