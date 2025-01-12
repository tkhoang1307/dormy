namespace Dormy.WebService.Api.Core.CustomExceptions
{
    public class UsernameIsExistedException(string message) : Exception(message)
    {
    }
}
