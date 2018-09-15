using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace _02___startup.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ShutdownMiddleware : IMiddleware
    {
        private readonly IApplicationLifetime _lifetime;

        public ShutdownMiddleware(IApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var action = context.Request.Query["action"];

            if (!string.IsNullOrWhiteSpace(action) 
               && action == "shutdown")
            {
                await context.Response.WriteAsync("Application is shutting down.");
                _lifetime.StopApplication();
                return;
            }

            await next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ShutdownMiddlewareExtensions
    {
        public static IApplicationBuilder UseShutdownMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ShutdownMiddleware>();
        }
    }
}
