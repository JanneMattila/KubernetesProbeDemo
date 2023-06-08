using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models;

public class HealthCheckModelResponse
{
    [JsonPropertyName("startup")]
    public bool StartupCheck { get; set; }

    [JsonPropertyName("readiness")]
    public bool ReadinessCheck { get; set; }

    [JsonPropertyName("liveness")]
    public bool LivenessCheck { get; set; }

    [JsonPropertyName("startupDelay")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int StartupDelay { get; set; }

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
