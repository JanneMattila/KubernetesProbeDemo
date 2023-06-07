using KubernetesProbeDemo.Models;
using System;

namespace KubernetesProbeDemo.Services;

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
        if (_healthCheckModel.LivenessDelayDuration != default &&
            _healthCheckModel.LivenessDelayDuration < DateTime.UtcNow)
        {
            _healthCheckModel.LivenessDelay = 0;
            _healthCheckModel.LivenessDelayDuration = default;
        }
        return _healthCheckModel;
    }

    public void Set(HealthCheckModelRequest healthCheckModel)
    {
        _healthCheckModel.LivenessCheck = healthCheckModel.LivenessCheck;
        _healthCheckModel.LivenessDelay = healthCheckModel.LivenessDelay;
        _healthCheckModel.ReadinessCheck = healthCheckModel.ReadinessCheck;
        _healthCheckModel.Shutdown = healthCheckModel.Shutdown;
        _healthCheckModel.LivenessDelayDuration = healthCheckModel.LivenessDelayDuration != 0 ?
            DateTime.UtcNow.AddSeconds(healthCheckModel.LivenessDelayDuration) : default;
    }
}
