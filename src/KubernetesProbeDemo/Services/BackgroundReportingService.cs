using KubernetesProbeDemo.Models;
using Microsoft.Extensions.Options;

namespace KubernetesProbeDemo.Services;

public class BackgroundReportingService : BackgroundService
{
    private readonly IWebhookHandler _webhookHandler;
    private readonly IHealthCheckRepository _healthCheckRepository;
    private readonly BackgroundReportingServiceOptions _options;

    public BackgroundReportingService(IWebhookHandler webhookHandler, IHealthCheckRepository healthCheckRepository,
        IHostApplicationLifetime appLifetime, IOptions<BackgroundReportingServiceOptions> options)
    {
        _webhookHandler = webhookHandler;
        _healthCheckRepository = healthCheckRepository;
        _options = options.Value;

        appLifetime.ApplicationStarted.Register(async () =>
        {
            await _webhookHandler.InvokeAsync(WebhookEvents.HostStarted, _healthCheckRepository.Get());
            Thread.Sleep(TimeSpan.FromSeconds(_options.DelayStartup));

            var model = _healthCheckRepository.Get();
            model.StartupCheck = true;
            await _webhookHandler.InvokeAsync(WebhookEvents.HostStartCompleted, model);
        });
        appLifetime.ApplicationStopping.Register(() =>
        {
            _webhookHandler.InvokeAsync(WebhookEvents.HostStopping, _healthCheckRepository.Get());
            Thread.Sleep(TimeSpan.FromSeconds(_options.DelayShutdown));
            _webhookHandler.InvokeAsync(WebhookEvents.HostStoppingCompleted, _healthCheckRepository.Get());
        });
        appLifetime.ApplicationStopped.Register(async () =>
            await _webhookHandler.InvokeAsync(WebhookEvents.HostStopped, _healthCheckRepository.Get()));
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
