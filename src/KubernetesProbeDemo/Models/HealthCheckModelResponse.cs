﻿using System;
using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models
{
    public class HealthCheckModelResponse
    {
        [JsonPropertyName("readiness")]
        public bool ReadinessCheck { get; set; }

        [JsonPropertyName("liveness")]
        public bool LivenessCheck { get; set; }

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
    }
}
