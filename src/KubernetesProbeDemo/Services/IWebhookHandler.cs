using KubernetesProbeDemo.Models;
using System.Threading.Tasks;

namespace KubernetesProbeDemo.Services;

public interface IWebhookHandler
{
    Task InvokeAsync(string invokeEvent, HealthCheckModelResponse healthCheckModel);
}