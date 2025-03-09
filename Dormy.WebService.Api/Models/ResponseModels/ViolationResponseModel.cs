using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ViolationResponseModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime ViolationDate { get; set; }

        public decimal Penalty { get; set; }

        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalIdNumber { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
    }
}
