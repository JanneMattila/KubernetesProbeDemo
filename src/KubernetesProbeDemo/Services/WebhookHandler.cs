using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace KubernetesProbeDemo.Services;

public class WebhookHandler : IWebhookHandler
{
    private readonly HttpClient _client = new();
    private readonly bool _enabled;

    public WebhookHandler(string? webhookUrl)
    {
        if (!string.IsNullOrEmpty(webhookUrl))
        {
            _client.BaseAddress = new Uri(webhookUrl);
            _enabled = true;
        }
    }

    public async Task InvokeAsync(string invokeEvent, object data)
    {
        if (_enabled)
        {
            var json = JsonSerializer.Serialize(data);
            using var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            await _client.PostAsync($"?invoke={invokeEvent}", content);
        }
    }
}
