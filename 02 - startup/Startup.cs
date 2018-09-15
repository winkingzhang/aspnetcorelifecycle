using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using _02___startup.Middlewares;
using Microsoft.Extensions.ObjectPool;

namespace _02___startup
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly ILoggerFactory _loggerFactory;

        // this injection be handled by IWebHostBuilder
        public Startup(IConfiguration config, ILoggerFactory loggerFactory)
        {
            _config = config;
            _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
            });
            //services.AddScoped<CustomerTracker>()

            services.AddTransient<ShutdownMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // demo 3
            // ---------------------------
            app.UseShutdownMiddleware();

            app.UseStaticFiles("/public");
            // ---------------------------

            app.UseSession();

            app.Map("/endpoint", (builder) =>
            {
                builder.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Hello in Map");
                });
            });

            // demo 3
            // ---------------------------
            app.Map("/objectPool", (builder) =>
            {
                builder.Run(async (context) =>
                {
                    var objPool = app.ApplicationServices.GetService<ObjectPoolProvider>().Create<BigObject>();

                    Parallel.For(0, 100, i =>
                    {
                        var o = objPool.Get();
                        Task.Delay(2).ContinueWith((arg) => objPool.Return(o));
                    });
                    await context.Response.WriteAsync("Memory: " + GC.GetTotalMemory(false).ToString());
                });
            });
            // ---------------------------

            // This is the last fallback middleware
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello in last Run!");

            });
        }


        internal class BigObject
        {
            public int[] value = new int[1024 * 100];
        }
    }
}
