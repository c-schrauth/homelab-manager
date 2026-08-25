using System.Diagnostics;
using HomelabManager.Core.Health;

namespace HomelabManager.Infrastructure.Health;

/// <summary>
/// Performs health checks against HTTP endpoints.
/// </summary>
public sealed class HttpHealthChecker : IHealthChecker
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpHealthChecker"/> class.
    /// </summary>
    /// <param name="httpClient">HTTP client used for health checks.</param>
    public HttpHealthChecker(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckAsync(Uri endpoint, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            using var response = await _httpClient.GetAsync(endpoint, cancellationToken);

            stopwatch.Stop();

            if (response.IsSuccessStatusCode)
                return HealthCheckResult.Healthy(stopwatch.Elapsed);

            return HealthCheckResult.Unhealthy(stopwatch.Elapsed, $"HTTP {(int)response.StatusCode} ({response.StatusCode})");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            stopwatch.Stop();

            return HealthCheckResult.Unhealthy(stopwatch.Elapsed, ex.Message);
        }
    }
}