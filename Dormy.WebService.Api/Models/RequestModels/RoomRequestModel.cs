using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RoomRequestModel
    {
        public string RoomNumber { get; set; } = string.Empty;

        public int TotalAvailableBed { get; set; }

        public RoomStatusEnum RoomStatus { get; set; } = RoomStatusEnum.UNDER_MAINTENANCE;

        public Guid RoomTypeId { get; set; }
    }
}
