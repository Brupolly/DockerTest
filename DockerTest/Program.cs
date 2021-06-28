using MassTransit;
using MassTransit.Definition;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sample.Service;

using Serilog;

using System;
using System.Text;
using System.Threading.Tasks;

namespace DockerTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            using var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            if (args != null)
            {
                configBuilder.AddCommandLine(args);
            }

            var config = configBuilder.Build();

            return new HostBuilder()
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.Sources.Clear();
                    cfg.AddConfiguration(config);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddSerilog(dispose: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
                    services.AddMassTransit(cfg =>
                    {
                        cfg.UsingRabbitMq((busContext, rmqCfg) => rmqCfg.ConfigureEndpoints(busContext));
                    });

                    services.AddHostedService<MassTransitService>();
                });
        }
    }
}
