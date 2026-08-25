using System.Net;
using HomelabManager.Core.Models;
using HomelabManager.Infrastructure.Health;

namespace HomelabManager.UnitTests.Infrastructure.Health;

/// <summary>
/// Tests for <see cref="HttpHealthChecker"/>.
/// </summary>
public class HttpHealthCheckerTests
{
    [Fact]
    public async Task CheckAsync_WhenEndpointReturnsSuccess_ShouldReturnHealthy()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.OK);

        using var httpClient = new HttpClient(handler);
        var checker = new HttpHealthChecker(httpClient);

        var result = await checker.CheckAsync(new Uri("https://example.test"));

        Assert.Equal(ServiceStatus.Healthy, result.Status);
        Assert.Null(result.ErrorMessage);
        Assert.True(result.Duration >= TimeSpan.Zero);
    }

    [Fact]
    public async Task CheckAsync_WhenEndpointReturnsError_ShouldReturnUnhealthy()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.InternalServerError);

        using var httpClient = new HttpClient(handler);
        var checker = new HttpHealthChecker(httpClient);

        var result = await checker.CheckAsync(new Uri("https://example.test"));

        Assert.Equal(ServiceStatus.Unhealthy, result.Status);
        Assert.Contains("500", result.ErrorMessage);
    }

    [Fact]
    public async Task CheckAsync_WhenRequestFails_ShouldReturnUnhealthy()
    {
        var handler = new FailingHttpMessageHandler();

        using var httpClient = new HttpClient(handler);
        var checker = new HttpHealthChecker(httpClient);

        var result = await checker.CheckAsync(new Uri("https://example.test"));

        Assert.Equal(ServiceStatus.Unhealthy, result.Status);
        Assert.Contains("Connection failed", result.ErrorMessage);
    }

    [Fact]
    public async Task CheckAsync_WhenRequestIsCancelled_ShouldReturnUnhealthy()
    {
        var handler = new CacellingHttpMessageHandler();

        using var httpClient = new HttpClient(handler);
        var checker = new HttpHealthChecker(httpClient);

        var result = await checker.CheckAsync(new Uri("https://example.test"));

        Assert.Equal(ServiceStatus.Unhealthy, result.Status);
    }

    private sealed class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;

        public TestHttpMessageHandler(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                RequestMessage = request
            };

            return Task.FromResult(response);
        }
    }

    private sealed class FailingHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new HttpRequestException("Connection failed");
        }
    }

    private sealed class CacellingHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new TaskCanceledException("Request time out.");
        }
    }
}