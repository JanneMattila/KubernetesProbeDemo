using KubernetesProbeDemo.Models;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KubernetesProbeDemo.Services
{
    public class WebhookHandler : IWebhookHandler
    {
        private HttpClient _client = new HttpClient();

        public WebhookHandler(string webhookUrl)
        {
            _client.BaseAddress = new Uri(webhookUrl);
        }

        public async Task InvokeAsync(string invokeEvent, HealthCheckModel healthCheckModel)
        {
            var json = JsonSerializer.Serialize(healthCheckModel);
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            await _client.PostAsync($"?invoke={invokeEvent}", content);
        }
    }
}
