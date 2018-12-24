﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OdeToFood
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              IGreeter greeter,
                              ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use lower level of middleware implementation
            // The Use statement will only run once during the Startup
            app.Use(request =>
            {
                // the following is the middleware. It will be invoked once per HTTP request.
                return async (context) =>
                {
                    logger.LogInformation($"Request incoming");
                    if (context.Request.Path.StartsWithSegments("/mym"))
                    {
                        await context.Response.WriteAsync("HIT!");
                        logger.LogInformation($"Request handled");
                    }
                    else
                    {
                        await request(context);
                        logger.LogInformation($"Response outgoing");
                    }
                };
            });

            // "Use" Middleware
            app.UseWelcomePage(new WelcomePageOptions
            {
                Path = "/wp"
            });


            app.Run(async (context) =>
            {
                if (context.Request.Path.StartsWithSegments("/exception"))
                {
                    throw new Exception("REX Exception thrown");
                }

                var greeting = greeter.GetMessageOfTheDay();
                await context.Response.WriteAsync(greeting);
            });
        }
    }
}
