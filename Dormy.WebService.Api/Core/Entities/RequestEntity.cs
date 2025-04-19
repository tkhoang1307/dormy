using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class RequestEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public RequestStatusEnum Status { get; set; } = RequestStatusEnum.SUBMITTED;

        public string RequestType { get; set; } = string.Empty;

        public Guid? ApproverId { get; set; }

        public AdminEntity? Approver { get; set; } = null!;

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;

        public Guid? RoomId { get; set; }

        public RoomEntity? Room { get; set; } = null!;
    }
}
