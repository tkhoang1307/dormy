namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RoomTypeServiceCreationRequestModel
    {
        public Guid RoomTypeId { get; set; }

        public List<Guid> RoomServiceIds { get; set; } = null!;
    }

    public class RoomTypeServiceDeletionRequestModel
    {
        public Guid RoomTypeId { get; set; }

        public List<Guid> RoomServiceIds { get; set; } = null!;
    }

    public class RoomTypeServiceRequestModel
    {
        public Guid RoomTypeId { get; set; }

        public Guid RoomServiceId { get; set; }
    }
}
