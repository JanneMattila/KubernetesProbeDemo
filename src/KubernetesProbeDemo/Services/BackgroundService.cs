using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KubernetesProbeDemo.Services
{
    public class BackgroundService : IHostedService
    {
        private IWebhookHandler _webhookHandler;
        private IHealthCheckRepository _healthCheckRepository;

        public BackgroundService(IWebhookHandler webhookHandler, IHealthCheckRepository healthCheckRepository)
        {
            _webhookHandler = webhookHandler;
            _healthCheckRepository = healthCheckRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
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

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _webhookHandler.InvokeAsync(
                WebhookEvents.BackgroundServiceStopped,
                _healthCheckRepository.Get());
        }
    }
}
