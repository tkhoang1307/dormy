namespace Dormy.WebService.Api.Core.CustomExceptions
{
    public class EntityNotFoundException(string message) : Exception(message)
    {
    }
}
