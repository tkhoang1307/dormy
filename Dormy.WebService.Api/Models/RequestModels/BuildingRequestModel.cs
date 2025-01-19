using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class BuildingRequestModel
    {
        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        public GenderEnum GenderRestriction { get; set; }

        public List<FloorRequestModel> Floors { get; set; } = [];
    }

    public class FloorRequestModel
    {
        public int FloorId { get; set; }

        public List<RoomRequestModel> Rooms { get; set; } = [];
    }

    public class BuildingBatchRequestModel
    {
        public bool IsGetAll { get; set; } = false;
        public List<Guid> Ids { get; set; } = [];
    }
}
