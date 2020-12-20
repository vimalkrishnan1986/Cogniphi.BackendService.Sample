using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Logging;
using Microsoft.Extensions.Logging;
namespace Cogniphi.BackendService.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
            .Build()
            .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args).ConfigureLogging((builderContext, loggingBuilder) =>
            {
                // Set logging configuration based on values in Logging config section
                loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));

                // Add console and debug logging providers
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                // Add steeltoe dynamic logging provider
                loggingBuilder.AddDynamicConsole();
            })
                .UseStartup<Startup>();
            return builder;
        }
    }
}
