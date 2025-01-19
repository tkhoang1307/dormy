using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class BuildingResponseModel : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        public int TotalRooms { get; set; }

        public GenderEnum GenderRestriction { get; set; }

        public List<FloorResponseModel> Floors { get; set; } = [];

        public string CreatedByAdminName { get; set; } = string.Empty;

        public string UpdatedByAdminName { get; set; } = string.Empty;
    }

    public class FloorResponseModel
    {
        public int FloorId { get; set; }

        public List<RoomResponseModel> Rooms { get; set; } = [];
    }
}
