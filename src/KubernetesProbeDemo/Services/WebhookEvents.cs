namespace KubernetesProbeDemo.Services;

public static class WebhookEvents
{
    public const string AppStarted = "AppStarted";
    public const string AppStopped = "AppStopped";
    public const string AppException = "AppException";

    public const string HostStarted = "HostStarted";
    public const string HostStartCompleted = "HostStartCompleted";
    public const string HostStopping = "HostStopping";
    public const string HostStoppingCompleted = "HostStoppingCompleted";
    public const string HostStopped = "HostStopped";

    public const string BackgroundServiceStarted = "BackgroundServiceStarted";
    public const string BackgroundServiceProcessing = "BackgroundServiceProcessing";
    public const string BackgroundServiceStopped = "BackgroundServiceStopped";

    public const string HealthCheck = "HealthCheck";
    public const string StartupCheck = "StartupCheck";
    public const string LivenessCheck = "LivenessCheck";
    public const string ReadinessCheck = "ReadinessCheck";
    public const string Shutdown = "Shutdown";
}
