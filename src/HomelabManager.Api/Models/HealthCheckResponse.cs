namespace HomelabManager.Api.Models;

/// <summary>
/// Represents the API response of a health check.
/// </summary>
/// <param name="Status"></param>
/// <param name="Duration"></param>
/// <param name="ErrorMessage"></param>
public sealed record HealthCheckResponse(string Status, TimeSpan Duration, string? ErrorMessage);