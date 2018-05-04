using Newtonsoft.Json;
using System;

namespace KubernetesProbeDemo.Models
{
    public class HealthCheckModel
    {
        [JsonProperty(PropertyName = "readiness")]
        public bool ReadinessCheck { get; set; }

        [JsonProperty(PropertyName = "liveness")]
        public bool LivenessCheck { get; set; }

        [JsonProperty(PropertyName = "server")]
        public string Server
        {
            get
            {
                return Environment.MachineName;
            }
        }
    }
}
