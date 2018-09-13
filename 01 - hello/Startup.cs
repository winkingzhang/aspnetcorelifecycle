using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01___1min_demo.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

namespace _01___1min_demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<SampleMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // demo 2
            // ---------------------------
            app.UseSampleMiddleware();

            app.UseStaticFiles("/public");
            // ---------------------------

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello .NET Group!");

                // demo 2
                // ---------------------------
                var objPool = app.ApplicationServices.GetService<ObjectPoolProvider>().Create<BigObject>();

                Parallel.For(0, 100, i =>
                {
                    var o = objPool.Get();
                    Task.Delay(2).ContinueWith((arg) => objPool.Return(o));
                });
                await context.Response.WriteAsync("Memory: " + GC.GetTotalMemory(false).ToString());
                // ---------------------------
            });
        }

        internal class BigObject
        {

        }
    }
}
