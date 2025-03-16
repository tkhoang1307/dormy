namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class BuildingBatchResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        public int TotalRooms { get; set; }

        // GenderEnum
        public string GenderRestriction { get; set; } = string.Empty;
    }

    public class BuildingResponseModel : BuildingBatchResponseModel
    {
        public List<RoomBatchResponseModel> Rooms { get; set; } = [];
    }
}
