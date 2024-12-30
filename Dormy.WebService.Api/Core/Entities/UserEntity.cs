using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class UserEntity: BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalIdNumber { get; set; } = string.Empty;

        // Should define Enum for this status
        public string Status { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public Guid GuardianId { get; set; }

        public GuardianEntity Guardian { get; set; } = null!;

        public Guid WorkplaceId { get; set; }

        public WorkplaceEntity Workplace { get; set; } = null!;

        public Guid HealthInsuranceId { get; set; }

        public HealthInsuranceEntity HealthInsurance { get; set; } = null!;

        public List<ViolationEntity>? Violations { get; set; }

        public List<OvernightAbsenceEntity>? OvernightAbsences { get; set; }

        public List<NotificationEntity>? Notifications { get; set; }
    }
}
