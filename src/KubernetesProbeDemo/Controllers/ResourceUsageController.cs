using KubernetesProbeDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace KubernetesProbeDemo.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
public class ResourceUsageController : Controller
{
    /// <summary>
    /// Generates resource usage in the container.
    /// </summary>
    /// <remarks>
    /// Example resource usage request:
    ///
    ///     POST /api/ResourceUsage
    ///     {
    ///       "duration": true
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Resource usage request</param>
    /// <returns>OK</returns>
    /// <response code="200">Returns status ok</response>
    [HttpPost]
    [ProducesResponseType(typeof(ResourceUsageRequest), StatusCodes.Status200OK)]
    public ActionResult Post([FromBody] ResourceUsageRequest request)
    {
        var end = DateTime.UtcNow.AddSeconds(request.Duration);
        while (DateTime.UtcNow < end)
        {
            Thread.SpinWait(1_000_000);
        }
        return Ok();
    }
}
