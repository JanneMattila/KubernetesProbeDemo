namespace KubernetesProbeDemo.Services;

public interface IWebhookHandler
{
    Task InvokeAsync(string invokeEvent, object data);
}