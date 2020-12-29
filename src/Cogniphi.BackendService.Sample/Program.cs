using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

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
            var log4netRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(log4netRepository, new FileInfo("log4net.config"));

            var builder = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                           .AddJsonFile("appsettings.json")
                                                   .AddEnvironmentVariables();
                })
                .ConfigureLogging((builderContext, loggingBuilder) =>
                {
                    // Set logging configuration based on values in Logging config section
                    loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                })
            .UseStartup<Startup>();
            return builder;
        }
    }
}
