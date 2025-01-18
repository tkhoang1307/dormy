using Dormy.WebService.Api.Core.Interfaces;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class UserContextService : IUserContextService
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<string> UserRoles { get; set; } = [];
    }
}