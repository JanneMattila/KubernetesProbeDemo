using KubernetesProbeDemo.Models;
using KubernetesProbeDemo.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var webhook = builder.Configuration["webhook"];
var livenessCheck = builder.Configuration.GetValue<bool>("livenessCheck");
var readinessCheck = builder.Configuration.GetValue<bool>("readinessCheck");

// Following lines can throw if configuration is not correctly set.
// This correctly prevents container from starting in error scenario.
// Might throw->
var healthCheckModel = new HealthCheckModelResponse()
{
    LivenessCheck = livenessCheck,
    ReadinessCheck = readinessCheck
};

IHealthCheckRepository healthCheckRepository = new HealthCheckRepository();
IWebhookHandler webhookHandler = new WebhookHandler(webhook);
await webhookHandler.InvokeAsync(WebhookEvents.AppStarted, healthCheckRepository.Get());
// <-Might throw

builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddSingleton(webhookHandler);
builder.Services.AddSingleton(healthCheckRepository);

builder.Services.AddSingleton<IHostedService, BackgroundReportingService>();
builder.Services.AddMvc();

var app = builder.Build();

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

app.MapRazorPages();
app.MapControllers();

app.Run();
