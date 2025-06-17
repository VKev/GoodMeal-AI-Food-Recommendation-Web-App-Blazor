using Application;
using Blazor.Components;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

// Configure Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

// Add Razor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authorization
builder.Services.AddAuthorization();

// Configure Database from Environment Variables
var connectionString = GetConnectionStringFromEnvironment();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: GetEnvInt("DATABASE_MAX_RETRY_COUNT", 3));
        npgsqlOptions.CommandTimeout(
            GetEnvInt("DATABASE_COMMAND_TIMEOUT", 30));
    });

    if (environment.IsDevelopment())
    {
        options.EnableDetailedErrors(GetEnvBool("DATABASE_ENABLE_DETAILED_ERRORS", true));
        options.EnableSensitiveDataLogging(GetEnvBool("DATABASE_ENABLE_SENSITIVE_DATA_LOGGING", false));
    }
});

// Register Business Layer (Application + Domain)
builder.Services.AddApplication();

// Register Data Access Layer (Infrastructure)
builder.Services.AddInfrastructure();

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

// Helper methods for environment configuration
static string GetConnectionStringFromEnvironment()
{
    var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
    var database = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "goodmeal_db";
    var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
    var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "password";
    
    return $"Host={host};Port={port};Database={database};Username={username};Password={password};";
}

static int GetEnvInt(string key, int defaultValue)
{
    var value = Environment.GetEnvironmentVariable(key);
    return int.TryParse(value, out var result) ? result : defaultValue;
}

static bool GetEnvBool(string key, bool defaultValue)
{
    var value = Environment.GetEnvironmentVariable(key);
    return bool.TryParse(value, out var result) ? result : defaultValue;
}
