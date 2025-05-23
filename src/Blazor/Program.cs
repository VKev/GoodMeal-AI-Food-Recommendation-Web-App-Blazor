using Application;
using Blazor.Components;
using Blazor.Configs;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

// Configure Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

// Add Razor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authorization (optional, keeps parity with WebApi)
builder.Services.AddAuthorization();

// Database configuration
builder.Services.ConfigureOptions<DatabaseConfigSetup>();
builder.Services.AddDbContext<MyDbContext>((serviceProvider, options) =>
{
    var databaseConfig = serviceProvider.GetService<IOptions<DatabaseConfig>>()!.Value;
    options.UseNpgsql(databaseConfig.ConnectionString, actions =>
    {
        actions.EnableRetryOnFailure(databaseConfig.MaxRetryCount);
        actions.CommandTimeout(databaseConfig.CommandTimeout);
    });

    if (environment.IsDevelopment())
    {
        options.EnableDetailedErrors(databaseConfig.EnableDetailedErrors);
        options.EnableSensitiveDataLogging(databaseConfig.EnableSensitiveDataLogging);
    }
});

// Register application & infrastructure layers
builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// Update connection string in appsettings at runtime (helps local dev)
Infrastructure.Utils.AutoScaffold.UpdateAppSettingsFile("appsettings.json", "default");
Infrastructure.Utils.AutoScaffold.UpdateAppSettingsFile("appsettings.Development.json", "default");
