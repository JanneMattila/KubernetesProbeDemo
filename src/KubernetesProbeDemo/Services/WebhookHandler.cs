using KubernetesProbeDemo.Models;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KubernetesProbeDemo.Services;

public class WebhookHandler : IWebhookHandler
{
    private readonly HttpClient _client = new();
    private readonly bool _enabled;

    public WebhookHandler(string webhookUrl)
    {
        _enabled = !string.IsNullOrEmpty(webhookUrl);
        if (_enabled)
        {
            _client.BaseAddress = new Uri(webhookUrl);
        }
    }

    public async Task InvokeAsync(string invokeEvent, HealthCheckModelResponse healthCheckModel)
    {
        if (_enabled)
        {
            var json = JsonSerializer.Serialize(healthCheckModel);
            using var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            await _client.PostAsync($"?invoke={invokeEvent}", content);
        }
    }
}
