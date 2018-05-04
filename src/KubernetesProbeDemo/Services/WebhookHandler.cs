using KubernetesProbeDemo.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
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
            var json = JsonConvert.SerializeObject(healthCheckModel);
            var content = new StringContent(json, Encoding.UTF8, JsonMediaTypeFormatter.DefaultMediaType.MediaType);
            await _client.PostAsync($"?invoke={invokeEvent}", content);
        }
    }
}
