using Dormy.WebService.Api.Models.RequestModels;

namespace Dormy.WebService.Api.Core.Entities
{
    public class RoomServiceEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string RoomServiceName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public List<RoomTypeServiceEntity>? RoomTypeServices { get; set; }

        public List<ServiceIndicatorEntity>? ServiceIndicators { get; set; }
    }
}
