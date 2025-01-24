using Dormy.WebService.Api.Core.CustomExceptions;
using System.Net;

namespace Dormy.WebService.Api.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UsernameIsExistedException ex)
            {
                _logger.LogError($"Username is existed: {ex}");
                await HandleUsernameExistedExceptionAsync(httpContext, ex);
            }
            catch (DuplicatedPasswordUpdateException ex)
            {
                _logger.LogError($"Duplicated new password with current password: {ex}");
                await HandleDuplicatedPasswordExceptionAsync(httpContext, ex);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Duplicated new password with current password: {ex}");
                await HandleEntityNotFoundExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleUsernameExistedExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = exception.Message });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleDuplicatedPasswordExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = exception.Message });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleEntityNotFoundExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = exception.Message });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = exception.Message });
            return context.Response.WriteAsync(result);
        }
    }
}
