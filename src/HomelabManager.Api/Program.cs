using HomelabManager.Application.Services;
using HomelabManager.Core.Health;
using HomelabManager.Infrastructure.Health;
using HomelabManager.Infrastructure.Persistence;
using HomelabManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// DI
builder.Services.AddDbContext<ServiceDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("HomelabManager")));
builder.Services.AddHttpClient<IHealthChecker, HttpHealthChecker>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<CheckServiceHealth>();
builder.Services.AddScoped<GetServices>();
builder.Services.AddScoped<GetService>();
builder.Services.AddScoped<CreateService>();
builder.Services.AddScoped<UpdateService>();
builder.Services.AddScoped<DeleteService>();

// Add controllers to the container
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();

/// <summary>
/// Program used for integratoin tests.
/// </summary>
public partial class Program
{ }