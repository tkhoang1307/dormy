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

        public decimal Price { get; set; }

        // RoomStatusEnum
        public string Status { get; set; } = string.Empty;

        public Guid BuildingId { get; set; }

        public string BuildingName { get; set; } = string.Empty;

        public List<RoomServiceResponseModel> RoomServices { get; set; } = [];
    }
}
