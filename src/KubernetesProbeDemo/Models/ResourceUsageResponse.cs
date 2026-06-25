using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models;

public class ResourceUsageResponse
{
    [JsonPropertyName("totalAllocatedMemory")]
    public long TotalAllocatedMemory { get; set; }

    [JsonPropertyName("totalAllocatedMemoryText")]
    public string TotalAllocatedMemoryText => MemoryQuantity.Format(TotalAllocatedMemory);

    [JsonPropertyName("requestedMemory")]
    public long RequestedMemory { get; set; }

    [JsonPropertyName("requestedMemoryText")]
    public string RequestedMemoryText => MemoryQuantity.Format(RequestedMemory);

    [JsonPropertyName("error")]
    public bool Error { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}
