using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OdeToFood.Data;
using OdeToFood.Services;
using System;

namespace OdeToFood
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>();

            services.AddDbContext<OdeToFoodDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("OdeToFood")));

            services.AddScoped<IRestaurantData, SqlRestaurantData>(); // keep the object with the process of a HTTP Request.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              IGreeter greeter,
                              ILogger<Startup> logger)
        {
            // env can be set in lauchSettings.json
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Index");
                app.UseHsts();
            }

            app.UseMvc(ConfigurateRoutes);

            // Serve static file 
            // UseDefaultFiles should be in front of UseStaticFiles so it can return index.html per default path.
            app.UseStaticFiles(); // Only serve files when request path is exactly matched.

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
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Not Found");
            });
        }

        private void ConfigurateRoutes(IRouteBuilder routeBuilder)
        {
            // /Home/Index/4
            // Question Mark means the parameter is optional
            // Equal symbol means if controller is not specified, use "Home", and if action is not specified, use "Index".
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
