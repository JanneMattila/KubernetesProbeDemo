using System.Threading.Tasks;
using KubernetesProbeDemo.Models;

namespace KubernetesProbeDemo.Services
{
    public interface IWebhookHandler
    {
        Task InvokeAsync(string invokeEvent, HealthCheckModelResponse healthCheckModel);
    }
}