using Contracts;
using LoggerService;

namespace CompanyEmployees.Extensions;

public static class ServiceExtension
{
    public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader(); // allowing all for CORS
        });
    });

    public static void ConfigureIisIntegration(this IServiceCollection services) => services.Configure<IISOptions>(
        options =>
        {
            // default
        });

    public static void ConfigureLoggingService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();
}