using System.Threading;
using System.Threading.Tasks;

using MassTransit;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sample.Service
{
    public class MassTransitService : IHostedService
    {
        private readonly IBusControl _bus;
        private readonly ILogger<MassTransitService> _logger;

        public MassTransitService(IBusControl bus, ILogger<MassTransitService> logger)
        {
            _bus = bus;
            _logger = logger;
            _logger.LogInformation("Service created!");
        }

        public async Task StartAsync(CancellationToken cancellationToken) =>
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);

        public Task StopAsync(CancellationToken cancellationToken) =>
            _bus.StopAsync(cancellationToken);
    }
}