using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class AdminEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public GenderEnum Gender { get; set; }

        public List<OvernightAbsenceEntity>? OvernightAbsences { get; set; }

        public List<NotificationEntity>? Notifications { get; set; }

        public List<SettingEntity>? Settings { get; set; }

        public List<ServiceIndicatorEntity>? ServiceIndicators { get; set; }

        public List<ContractEntity>? Contracts { get; set; }

        public List<ContractExtensionEntity>? ContractExtensions { get; set; }

        public List<ParkingRequestEntity>? ParkingRequests { get; set; }

        public List<RequestEntity>? Requests { get; set; }
    }
}
