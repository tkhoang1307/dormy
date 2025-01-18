namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IUserContextService
    {
        Guid UserId { get; set; }
        string UserName { get; set; }
        List<string> UserRoles { get; set; }
    }
}
