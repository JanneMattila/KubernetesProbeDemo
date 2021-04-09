using System;
using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models
{
    public class HealthCheckModelResponse
    {
        [JsonPropertyName("readiness")]
        public bool ReadinessCheck { get; set; }

        [JsonPropertyName("liveness")]
        public bool LivenessCheck { get; set; }

        [JsonPropertyName("livenessDelay")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int LivenessDelay { get; set; }

        [JsonPropertyName("livenessDelayDuration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime LivenessDelayDuration { get; set; }

        [JsonPropertyName("shutdown")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Shutdown { get; set; }

        [JsonPropertyName("server")]
        public string Server
        {
            get
            {
                return Environment.MachineName;
            }
        }

        [JsonPropertyName("started")]
        public DateTime Started { get; set; }
    }
}
