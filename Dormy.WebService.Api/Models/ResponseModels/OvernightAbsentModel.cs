using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class OvernightAbsentModel
    {
        public Guid Id { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = OvernightAbsenceStatusEnum.SUBMITTED.ToString();

        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalIdNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
    }
}
