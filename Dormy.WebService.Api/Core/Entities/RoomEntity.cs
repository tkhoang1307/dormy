using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class RoomEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int TotalUsedBed { get; set; } = 0;

        public int TotalAvailableBed { get; set; }

        public RoomStatusEnum Status { get; set; } = RoomStatusEnum.AVAILABLE;

        public Guid BuildingId { get; set; }

        public BuildingEntity Building { get; set; } = null!;

        public Guid RoomTypeId { get; set; }

        public RoomTypeEntity RoomType { get; set; } = null!;

        public List<InvoiceEntity>? Invoices { get; set; }

        public List<ServiceIndicatorEntity>? ServiceIndicators { get; set; }

        public List<ContractEntity>? Contracts { get; set; }

        public List<RequestEntity>? Requests { get; set; }
    }
}
