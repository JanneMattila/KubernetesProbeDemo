using Newtonsoft.Json;

namespace KubernetesProbeDemo.Models
{
    public class HealthCheckModel
    {
        [JsonProperty(PropertyName = "readiness")]
        public bool ReadinessCheck { get; set; }

        [JsonProperty(PropertyName = "liveness")]
        public bool LivenessCheck { get; set; }
    }
}
