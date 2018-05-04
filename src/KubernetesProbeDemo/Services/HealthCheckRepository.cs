using KubernetesProbeDemo.Models;

namespace KubernetesProbeDemo.Services
{
    public class HealthCheckRepository : IHealthCheckRepository
    {
        private HealthCheckModel _healthCheckModel;

        public HealthCheckRepository()
        {
            _healthCheckModel = new HealthCheckModel()
            {
                LivenessCheck = true,
                ReadinessCheck = true
            };
        }

        public HealthCheckModel Get()
        {
            return _healthCheckModel;
        }

        public void Set(HealthCheckModel healthCheckModel)
        {
            _healthCheckModel = healthCheckModel;
        }
    }
}
