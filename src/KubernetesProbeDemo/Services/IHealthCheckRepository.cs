using KubernetesProbeDemo.Models;

namespace KubernetesProbeDemo.Services;

public interface IHealthCheckRepository
{
    HealthCheckModelResponse Get();
    void Set(HealthCheckModelRequest healthCheckModel);
}