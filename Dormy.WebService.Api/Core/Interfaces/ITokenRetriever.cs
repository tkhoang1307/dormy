namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface ITokenRetriever
    {
        string CreateToken(Guid id, string username, string email, string role);
    }
}
