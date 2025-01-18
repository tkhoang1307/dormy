using Dormy.WebService.Api.Core.Interfaces;
using System.Security.Claims;

namespace Dormy.WebService.Api.Infrastructure.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserContextService userContextService)
        {
            // Extract user context information from the request
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoles = context.User?.FindAll(ClaimTypes.Role)?.Select(r => r.Value).ToList();
            var username = context.User?.FindFirst(ClaimTypes.Name)?.Value;

            // Set the user context in the scoped service
            userContextService.UserId = Guid.Parse(userId ?? Guid.Empty.ToString());
            userContextService.UserRoles = userRoles ?? [];
            userContextService.UserName = username ?? string.Empty;

            await _next(context);
        }
    }

    // Extension method to add the middleware to the pipeline
    public static class UserContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserContextMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserContextMiddleware>();
        }
    }
}
