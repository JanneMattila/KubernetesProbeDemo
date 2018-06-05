using KubernetesProbeDemo.Models;
using KubernetesProbeDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KubernetesProbeDemo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Following lines can throw if configuration is not correctly set.
            // This correctly prevents container from starting in error scenario.
            // Might throw->
            var healthCheckModel = new HealthCheckModel()
            {
                LivenessCheck = Configuration.GetValue<bool>("livenessCheck"),
                ReadinessCheck = Configuration.GetValue<bool>("readinessCheck")
            };

            IHealthCheckRepository healthCheckRepository = new HealthCheckRepository();
            IWebhookHandler webhookHandler = new WebhookHandler(Configuration["webhook"]);
            webhookHandler.InvokeAsync(WebhookEvents.AppStarted, healthCheckRepository.Get()).Wait();
            // <-Might throw

            services.AddSingleton(webhookHandler);
            services.AddSingleton(healthCheckRepository);

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BackgroundReportingService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
