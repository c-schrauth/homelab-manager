using HomelabManager.Application.Services;
using HomelabManager.Core.Health;
using HomelabManager.IntegrationTests.TestDoubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HomelabManager.IntegrationTests;

public sealed class TestApplicationFactory : WebApplicationFactory<Program>
{
    public FakeServiceRepository ServiceRepository { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IServiceRepository>();
            services.RemoveAll<IHealthChecker>();

            services.AddSingleton<IServiceRepository>(ServiceRepository);
            services.AddSingleton<IHealthChecker>(new FakeHealthChecker());
        });
    }
}