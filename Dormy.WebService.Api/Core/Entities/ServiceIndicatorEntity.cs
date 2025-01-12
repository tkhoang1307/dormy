namespace Dormy.WebService.Api.Core.Entities
{
    public class ServiceIndicatorEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Month { get; set; }

        public int Year { get; set; }

        public decimal OldIndicator { get; set; }

        public decimal NewIndicator { get; set; }

        public string RoomServiceName { get; set; } = string.Empty;

        public Guid RoomServiceId { get; set; }

        public RoomServiceEntity RoomService { get; set; } = null!;

        public Guid RoomId { get; set; }

        public RoomEntity Room { get; set; } = null!;

        public Guid AdminId { get; set; }

        public AdminEntity Admin { get; set; } = null!;
    }
}
