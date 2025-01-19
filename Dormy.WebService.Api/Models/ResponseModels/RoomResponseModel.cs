using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RoomResponseModel : BaseEntity
    {
        public Guid Id { get; set; }

        public string RoomNumer { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        public int TotalAvailableBed { get; set; }

        public Guid RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = string.Empty;

        public RoomStatusEnum Status { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public Guid BuildingId { get; set; }
    }
}
