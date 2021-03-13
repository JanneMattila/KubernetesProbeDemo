using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models
{
    public class ResourceUsageRequest
    {
        [JsonPropertyName("duration")]
        public long Duration { get; set; }
    }
}
