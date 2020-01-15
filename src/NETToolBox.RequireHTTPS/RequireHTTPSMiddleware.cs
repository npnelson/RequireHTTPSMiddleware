using Microsoft.AspNetCore.Http;
using NETToolBox.RequireHTTPS;
using System.Threading.Tasks;

namespace NETToolBox.RequireHTTPS
{
#pragma warning disable S101 // Types should be named in PascalCase
    public sealed class RequireHTTPSMiddleware
#pragma warning restore S101 // Types should be named in PascalCase
    {

        private readonly RequestDelegate _next;
        public RequireHTTPSMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;              
            }
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class RequireHTTPSMiddlewareExtensions
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        /// Requires HTTPS NO MATTER WHAT!!! Non HTTPS requests will return 400 BadRequest 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder RequireHTTPS(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequireHTTPSMiddleware>();
        }
    }
}
