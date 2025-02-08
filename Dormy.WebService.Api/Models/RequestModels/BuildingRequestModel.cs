using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class BuildingRequestModel
    {
        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        // GenderEnum
        public string GenderRestriction { get; set; } = string.Empty;

        public List<RoomRequestModel> Rooms { get; set; } = [];
    }

    public class BuildingCreationRequestModel
    {
        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        // GenderEnum
        public string GenderRestriction { get; set; } = string.Empty;

        public List<RoomCreationRequestModel> Rooms { get; set; } = [];
    }
}
