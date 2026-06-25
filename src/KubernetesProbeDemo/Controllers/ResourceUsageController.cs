using KubernetesProbeDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace KubernetesProbeDemo.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
public class ResourceUsageController : Controller
{
    private static readonly List<byte[]> _memoryAllocations = new();
    private static readonly object _memoryLock = new();

    // A single array cannot exceed ~2 GB, so large allocations are split into chunks.
    private const int ChunkSize = 64 * 1024 * 1024;

    /// <summary>
    /// Generates resource usage in the container.
    /// </summary>
    /// <remarks>
    /// Example resource usage request:
    ///
    ///     POST /api/ResourceUsage
    ///     {
    ///       "duration": 1,
    ///       "memory": "100MB"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Resource usage request</param>
    /// <returns>OK</returns>
    /// <response code="200">Returns status ok</response>
    [HttpPost]
    [ProducesResponseType(typeof(ResourceUsageResponse), StatusCodes.Status200OK)]
    public ActionResult Post([FromBody] ResourceUsageRequest request)
    {
        if (request.Duration > 0)
        {
            var end = DateTime.UtcNow.AddSeconds(request.Duration);
            while (DateTime.UtcNow < end)
            {
                Thread.SpinWait(1_000_000);
            }
        }

        var response = new ResourceUsageResponse();

        if (!string.IsNullOrWhiteSpace(request.Memory))
        {
            if (MemoryQuantity.TryParse(request.Memory, out var requestedMemory))
            {
                response.RequestedMemory = requestedMemory;
                try
                {
                    var remaining = requestedMemory;
                    while (remaining > 0)
                    {
                        var size = (int)Math.Min(remaining, ChunkSize);
                        var allocation = new byte[size];
                        // Touch every page so the memory is actually committed.
                        for (var i = 0; i < allocation.Length; i += 4096)
                        {
                            allocation[i] = 1;
                        }

                        lock (_memoryLock)
                        {
                            _memoryAllocations.Add(allocation);
                        }

                        remaining -= size;
                    }
                }
                catch (Exception ex)
                {
                    response.Error = true;
                    response.Message = $"Exception while allocating {request.Memory} ({requestedMemory} bytes): {ex.Message}";
                }
            }
            else
            {
                response.Error = true;
                response.Message = $"'{request.Memory}' is not a valid memory quantity.";
            }
        }

        lock (_memoryLock)
        {
            response.TotalAllocatedMemory = _memoryAllocations.Sum(allocation => (long)allocation.Length);
        }

        return Ok(response);
    }

    /// <summary>
    /// Releases all the allocated memory.
    /// </summary>
    /// <remarks>
    /// Example release request:
    ///
    ///     DELETE /api/ResourceUsage
    ///
    /// </remarks>
    /// <returns>OK</returns>
    /// <response code="200">Returns status ok</response>
    [HttpDelete]
    [ProducesResponseType(typeof(ResourceUsageResponse), StatusCodes.Status200OK)]
    public ActionResult Delete()
    {
        var response = new ResourceUsageResponse();

        lock (_memoryLock)
        {
            response.RequestedMemory = _memoryAllocations.Sum(allocation => (long)allocation.Length);
            _memoryAllocations.Clear();
        }

        // Reclaim the released memory.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        return Ok(response);
    }
}
