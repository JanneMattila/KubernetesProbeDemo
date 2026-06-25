using System.Text.Json.Serialization;

namespace KubernetesProbeDemo.Models;

public class ResourceUsageResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("totalAllocatedMemory")]
    public long TotalAllocatedMemory { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("totalAllocatedMemoryText")]
    public string? TotalAllocatedMemoryText => TotalAllocatedMemory == 0 ? null : MemoryQuantity.Format(TotalAllocatedMemory);

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("requestedMemory")]
    public long RequestedMemory { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("requestedMemoryText")]
    public string? RequestedMemoryText => RequestedMemory == 0 ? null : MemoryQuantity.Format(RequestedMemory);

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("machineMemoryUsage")]
    public long MachineMemoryUsage { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("machineMemoryUsageText")]
    public string? MachineMemoryUsageText => MachineMemoryUsage == 0 ? null : MemoryQuantity.Format(MachineMemoryUsage);

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("availableMemory")]
    public long AvailableMemory { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("availableMemoryText")]
    public string? AvailableMemoryText => AvailableMemory == 0 ? null : MemoryQuantity.Format(AvailableMemory);

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("memoryUsagePercentage")]
    public double MemoryUsagePercentage { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("error")]
    public bool Error { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
