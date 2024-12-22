using Dormy.WebService.Api.Models.Enums;
using System.Reflection;

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

        public Gender Gender { get; set; }
    }
}
