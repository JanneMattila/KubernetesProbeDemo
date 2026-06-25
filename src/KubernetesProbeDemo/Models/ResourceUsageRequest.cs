using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models;

public class ResourceUsageRequest
{
    [JsonPropertyName("duration")]
    public long Duration { get; set; }

    [JsonPropertyName("memory")]
    public string Memory { get; set; } = string.Empty;
}
