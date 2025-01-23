using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class OvernightAbsenceEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public DateTime EndDateTime { get; set; } = DateTime.Now.AddDays(1);

        public string Reason { get; set; } = string.Empty;

        public OvernightAbsenceStatusEnum Status { get; set; } = OvernightAbsenceStatusEnum.SUBMITTED;

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;

        public Guid AdminId { get; set; }

        public AdminEntity Admin { get; set; } = null!;


    }
}
