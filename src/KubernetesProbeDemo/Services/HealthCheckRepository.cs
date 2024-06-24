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
            StartupCheck = true,
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
        _healthCheckModel.StartupCheck = healthCheckModel.StartupCheck;
        _healthCheckModel.StartupStatusCode = healthCheckModel.StartupStatusCode;
        _healthCheckModel.LivenessCheck = healthCheckModel.LivenessCheck;
        _healthCheckModel.LivenessStatusCode = healthCheckModel.LivenessStatusCode;
        _healthCheckModel.LivenessDelay = healthCheckModel.LivenessDelay;
        _healthCheckModel.ReadinessCheck = healthCheckModel.ReadinessCheck;
        _healthCheckModel.ReadinessStatusCode = healthCheckModel.ReadinessStatusCode;
        _healthCheckModel.Shutdown = healthCheckModel.Shutdown;
        _healthCheckModel.LivenessDelayDuration = healthCheckModel.LivenessDelayDuration != 0 ?
            DateTime.UtcNow.AddSeconds(healthCheckModel.LivenessDelayDuration) : default;
    }
}
