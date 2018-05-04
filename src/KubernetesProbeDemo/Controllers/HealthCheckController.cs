using KubernetesProbeDemo.Models;
using KubernetesProbeDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace KubernetesProbeDemo.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        private IWebhookHandler _webhookHandler;
        private IHealthCheckRepository _healthCheckRepository;

        public HealthCheckController(IWebhookHandler webhookHandler, IHealthCheckRepository healthCheckRepository)
        {
            _webhookHandler = webhookHandler;
            _healthCheckRepository = healthCheckRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_healthCheckRepository.Get());
        }

        [HttpGet("Liveness")]
        public async Task<ActionResult> GetLiveness()
        {
            var healthCheck = _healthCheckRepository.Get();
            await _webhookHandler.InvokeAsync(
                WebhookEvents.LivenessCheck,
                healthCheck);

            if (healthCheck.LivenessCheck)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

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

        [HttpPost]
        public ActionResult Post([FromBody]HealthCheckModel healthCheckModel)
        {
            _healthCheckRepository.Set(healthCheckModel);
            return Ok(_healthCheckRepository.Get());
        }
    }
}
