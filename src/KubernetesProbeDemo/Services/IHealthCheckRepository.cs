using KubernetesProbeDemo.Models;

namespace KubernetesProbeDemo.Services
{
    public interface IHealthCheckRepository
    {
        HealthCheckModel Get();
        void Set(HealthCheckModel healthCheckModel);
    }
}