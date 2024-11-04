using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Contracts;
using Services;

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

    // Singleton - Instantiated once at runtime
    public static void ConfigureLoggingService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    // Scoped - Instantiated every use
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("sqlConnection")); // no need for migrations
        });
}