namespace Dormy.WebService.Api.Core.CustomExceptions
{
    public class DuplicatedPasswordUpdateException(string message) : Exception(message)
    {
    }
}
