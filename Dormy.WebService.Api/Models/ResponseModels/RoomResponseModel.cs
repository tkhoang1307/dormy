namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RoomBatchResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int TotalAvailableBed { get; set; }

        public int TotalUsedBed { get; set; }

        public Guid RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Status { get; set; } = string.Empty;

        public Guid BuildingId { get; set; }

        public string BuildingName { get; set; } = string.Empty;
    }

    public class RoomResponseModel : RoomBatchResponseModel
    {
        public List<RoomServiceResponseModel> RoomServices { get; set; } = [];
        public List<UserResponseModel> Users { get; set; } = [];
    }
}
