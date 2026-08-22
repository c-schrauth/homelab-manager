namespace HomelabManager.Agent;

/// <summary>
/// Background worker responsible for executing agent tasks.
/// </summary>
public sealed class Worker(ILogger<Worker> logger) : BackgroundService
{
    /// <summary>
    /// Executes the background worker.
    /// </summary>
    /// <param name="stoppingToken">Token used to request graceful shutdown.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Homelab Manager Agent is running.");
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
