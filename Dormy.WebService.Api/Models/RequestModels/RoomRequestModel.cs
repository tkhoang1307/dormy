using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RoomRequestModel
    {
        public int RoomNumber { get; set; }

        public int TotalAvailableBed { get; set; }

        public RoomStatusEnum RoomStatus { get; set; } = RoomStatusEnum.AVAILABLE;

        public Guid RoomTypeId { get; set; }

        public int FloorNumber { get; set; }
    }

    public class RoomCreationRequestModel
    {
        public Guid RoomTypeId { get; set; }

        public int FloorNumber { get; set; }

        public int TotalRoomsWantToCreate { get; set; }
    }

    public class RoomUpdateRequestModel
    {
        public Guid Id { get; set; }

        public Guid RoomTypeId { get; set; }

        public int FloorNumber { get; set; }
    }

    public class RoomUpdateStatusRequestModel
    {
        public Guid Id { get; set; }

        public RoomStatusEnum Status { get; set; }
    }
}
