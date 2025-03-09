namespace Dormy.WebService.Api.Core.CustomExceptions
{
    public class BadRequestException(string message) : Exception(message)
    {
    }
}
