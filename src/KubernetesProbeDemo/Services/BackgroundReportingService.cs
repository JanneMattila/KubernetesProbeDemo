using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KubernetesProbeDemo.Services
{
    public class BackgroundReportingService : BackgroundService
    {
        private readonly IWebhookHandler _webhookHandler;
        private readonly IHealthCheckRepository _healthCheckRepository;

        public BackgroundReportingService(IWebhookHandler webhookHandler, IHealthCheckRepository healthCheckRepository)
        {
            _webhookHandler = webhookHandler;
            _healthCheckRepository = healthCheckRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _webhookHandler.InvokeAsync(
                WebhookEvents.BackgroundServiceStarted, 
                _healthCheckRepository.Get());

            while (!cancellationToken.IsCancellationRequested)
            {
                await _webhookHandler.InvokeAsync(
                    WebhookEvents.BackgroundServiceProcessing,
                    _healthCheckRepository.Get());
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _webhookHandler.InvokeAsync(
                WebhookEvents.BackgroundServiceStopped,
                _healthCheckRepository.Get());
        }
    }
}
