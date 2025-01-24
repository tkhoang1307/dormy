namespace Dormy.WebService.Api.Core.Entities
{
    public class RoomTypeServiceEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid RoomTypeId { get; set; }

        public RoomTypeEntity RoomType { get; set; } = null!;

        public Guid RoomServiceId { get; set; }

        public RoomServiceEntity RoomService { get; set; } = null!;
    }
}
