using KubernetesProbeDemo.Models;
using KubernetesProbeDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KubernetesProbeDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        private readonly IWebhookHandler _webhookHandler;
        private readonly IHealthCheckRepository _healthCheckRepository;

        public HealthCheckController(IWebhookHandler webhookHandler, IHealthCheckRepository healthCheckRepository)
        {
            _webhookHandler = webhookHandler;
            _healthCheckRepository = healthCheckRepository;
        }

        /// <summary>
        /// Get service health check status.
        /// </summary>
        /// <remarks>
        /// Example service health check status request:
        ///
        ///     GET /api/HealthCheck
        ///
        /// </remarks>
        /// <returns>Current service health check status</returns>
        /// <response code="200">Returns service health check status</response>
        /// <response code="503">Returns service unavailable</response>
        [ProducesResponseType(typeof(HealthCheckModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_healthCheckRepository.Get());
        }

        /// <summary>
        /// Get liveness probe status.
        /// </summary>
        /// <remarks>
        /// Example liveness check request:
        ///
        ///     GET /api/HealthCheck/Liveness
        ///
        /// </remarks>
        /// <returns>Current liveness data</returns>
        /// <response code="200">Returns liveness status</response>
        /// <response code="503">Returns service unavailable</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [HttpGet("Liveness")]
        public async Task<ActionResult> GetLiveness()
        {
            var healthCheck = _healthCheckRepository.Get();
            await _webhookHandler.InvokeAsync(
                WebhookEvents.LivenessCheck,
                healthCheck);

            if (healthCheck.LivenessDelay > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(healthCheck.LivenessDelay));
            }

            if (healthCheck.LivenessCheck)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        /// <summary>
        /// Get readiness probe status.
        /// </summary>
        /// <remarks>
        /// Example readiness check request:
        ///
        ///     GET /api/HealthCheck/Readiness
        ///
        /// </remarks>
        /// <returns>Current readiness data</returns>
        /// <response code="200">Returns readiness status</response>
        /// <response code="503">Returns service unavailable</response>
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [HttpGet("Readiness")]
        public async Task<ActionResult> GetReadiness()
        {
            var healthCheck = _healthCheckRepository.Get();
            await _webhookHandler.InvokeAsync(
                WebhookEvents.ReadinessCheck,
                healthCheck);

            if (healthCheck.ReadinessCheck)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        /// <summary>
        /// Updates service health check status.
        /// </summary>
        /// <remarks>
        /// Example health check update request:
        ///
        ///     POST /api/HealthCheck
        ///     {
        ///       "readiness": true,
        ///       "liveness": true,
        ///       "shutdown": false
        ///     }
        ///
        /// NOTE: If you set "shutdown"  to true, then service
        ///       is exited.
        /// </remarks>
        /// <param name="request">Updated health check data</param>
        /// <returns>Current health check data</returns>
        /// <response code="200">Returns health check data</response>
        /// <response code="409">Returns health check data but does not change state</response>
        [HttpPost]
        [ProducesResponseType(typeof(HealthCheckModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HealthCheckModelResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Post([FromBody] HealthCheckModelRequest request)
        {
            if (!string.IsNullOrEmpty(request.Condition))
            {
                if (request.Condition != Environment.MachineName)
                {
                    return Conflict(_healthCheckRepository.Get());
                }
            }

            _healthCheckRepository.Set(request);

            if (request.Shutdown)
            {
                await _webhookHandler.InvokeAsync(
                    WebhookEvents.Shutdown,
                    _healthCheckRepository.Get());

                // Exit this process with failure code
                Environment.Exit(123);
            }

            return Ok(_healthCheckRepository.Get());
        }
    }
}
