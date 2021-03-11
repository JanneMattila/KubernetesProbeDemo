using KubernetesProbeDemo.Models;
using KubernetesProbeDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace KubernetesProbeDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Following lines can throw if configuration is not correctly set.
            // This correctly prevents container from starting in error scenario.
            // Might throw->
            var healthCheckModel = new HealthCheckModelResponse()
            {
                LivenessCheck = Configuration.GetValue<bool>("livenessCheck"),
                ReadinessCheck = Configuration.GetValue<bool>("readinessCheck")
            };

            IHealthCheckRepository healthCheckRepository = new HealthCheckRepository();
            IWebhookHandler webhookHandler = new WebhookHandler(Configuration["webhook"]);
            webhookHandler.InvokeAsync(WebhookEvents.AppStarted, healthCheckRepository.Get()).Wait();
            // <-Might throw

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "K8s probe demo API",
                    Description = "Kubernetes liveness and readiness demos",
                    Contact = new OpenApiContact()
                    {
                        Name = "GitHub",
                        Url = new Uri("https://github.com/JanneMattila/KubernetesProbeDemo")
                    },
                    TermsOfService = new Uri("https://github.com/JanneMattila/KubernetesProbeDemo"),
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton(webhookHandler);
            services.AddSingleton(healthCheckRepository);

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BackgroundReportingService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "K8s probe demo API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
