using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace OdeToFood
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args) // this will inject the basic service, such as IConfiguration
                .UseStartup<Startup>(); // So, the Startup class is injectable.
    }
}
