using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class BuildingRequestModel
    {
        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        public GenderEnum GenderRestriction { get; set; }

        public List<RoomRequestModel> Rooms { get; set; } = [];
    }
}
