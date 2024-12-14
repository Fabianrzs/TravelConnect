using Microsoft.AspNetCore.Builder;
using TravelConnect.Infrastructure.Middleware;

namespace TravelConnect.Infrastructure.Extensions;

public static class MiddlewareExtensions
{
    public static void UseExceptionMiddlewareApp(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
