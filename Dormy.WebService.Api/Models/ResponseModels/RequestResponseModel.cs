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
        public int? RoomNumber { get; set; }
        public int? FloorNumber { get; set; }
        public string? RoomStatus { get; set; } = string.Empty;
        public Guid? BuildingId { get; set; }
        public string? BuildingName { get; set; } = string.Empty;
    }
}
