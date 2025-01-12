using Dormy.WebService.Api.Models.Enums;
using System.Buffers.Text;

namespace Dormy.WebService.Api.Core.Entities
{
    public class ParkingRequestEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public RequestStatusEnum Status { get; set; } = RequestStatusEnum.SUBMITTED;

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;

        public Guid ParkingSpotId { get; set; }

        public ParkingSpotEntity ParkingSpot { get; set; } = null!;

        public Guid VehicleId { get; set; }

        public VehicleEntity Vehicle { get; set; } = null!;

        public Guid ApproverId { get; set; }

        public AdminEntity Approver { get; set; } = null!;
    }
}
