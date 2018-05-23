namespace KubernetesProbeDemo.Services
{
    public static class WebhookEvents
    {
        public const string AppStarted = "AppStarted";

        public const string BackgroundServiceStarted = "BackgroundServiceStarted";
        public const string BackgroundServiceProcessing = "BackgroundServiceProcessing";
        public const string BackgroundServiceStopped = "BackgroundServiceStopped";

        public const string HealthCheck = "HealthCheck";
        public const string LivenessCheck = "LivenessCheck";
        public const string ReadinessCheck = "ReadinessCheck";
        public const string Shutdown = "Shutdown";
    }
}
