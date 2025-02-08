using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RoomResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int TotalAvailableBed { get; set; }

        public Guid RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = string.Empty;

        // RoomStatusEnum
        public string Status { get; set; } = string.Empty;

        public Guid BuildingId { get; set; }
    }
}
