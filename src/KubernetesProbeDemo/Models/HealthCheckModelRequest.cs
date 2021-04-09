using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models
{
    public class HealthCheckModelRequest
    {
        [JsonPropertyName("readiness")]
        public bool ReadinessCheck { get; set; }

        [JsonPropertyName("liveness")]
        public bool LivenessCheck { get; set; }

        [JsonPropertyName("livenessDelay")]
        public int LivenessDelay { get; set; }

        [JsonPropertyName("livenessDelayDuration")]
        public int LivenessDelayDuration { get; set; }

        [JsonPropertyName("shutdown")]
        public bool Shutdown { get; set; }

        [JsonPropertyName("condition")]
        public string Condition { get; set; }
    }
}
