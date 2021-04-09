using KubernetesProbeDemo.Models;
using System;

namespace KubernetesProbeDemo.Services
{
    public class HealthCheckRepository : IHealthCheckRepository
    {
        private readonly HealthCheckModelResponse _healthCheckModel;

        public HealthCheckRepository()
        {
            _healthCheckModel = new HealthCheckModelResponse()
            {
                LivenessCheck = true,
                ReadinessCheck = true,
                Started = DateTime.UtcNow
            };
        }

        public HealthCheckModelResponse Get()
        {
            return _healthCheckModel;
        }

        public void Set(HealthCheckModelRequest healthCheckModel)
        {
            _healthCheckModel.LivenessCheck = healthCheckModel.LivenessCheck;
            _healthCheckModel.LivenessDelay = healthCheckModel.LivenessDelay;
            _healthCheckModel.ReadinessCheck = healthCheckModel.ReadinessCheck;
            _healthCheckModel.Shutdown = healthCheckModel.Shutdown;
        }
    }
}
