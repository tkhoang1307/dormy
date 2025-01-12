using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class BedEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int BedNumber { get; set; }
        public BedStatusEnum Status { get; set; }

        public Guid RoomId { get; set; }

        public RoomEntity Room { get; set; } = null!;

        public List<ContractEntity>? Contracts { get; set; }
    }
}
