namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RoomServiceRequestModel
    {
        public string RoomServiceName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Cost { get; set; }
    }

    public class RoomServiceUpdateRequestModel : RoomServiceRequestModel
    {
        public Guid Id { get; set; }
    }
}
