namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RoomRequestModel
    {
        public string RoomNumber { get; set; } = string.Empty;

        public int TotalAvailableBed { get; set; }

        public Guid RoomTypeId { get; set; }
    }
}
