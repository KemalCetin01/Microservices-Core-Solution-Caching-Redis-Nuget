using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MS.Services.Core.Caching.Redis.Helper;
using MS.Services.Core.Caching.Redis.Service.Concrete;
using MS.Services.Core.Caching.Redis.Service.Interface;

namespace MS.Services.Core.Caching.Redis;

public static class ServiceRegistration
{
    public static void AddRedisCache(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddOptions<RedisOption>()
            .Configure(options => configuration.GetSection("Redis").Bind(options))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        serviceCollection.AddSingleton<IRedisServer, RedisServer>();
        serviceCollection.AddSingleton<IRedisCacheService, RedisCacheService>();

        // Add Redis health check
        serviceCollection.AddHealthChecks()
            .AddRedis(configuration.GetSection("Redis:Server").Get<string>()!,
                name: "Redis Server Check",
                failureStatus: HealthStatus.Degraded,
                tags: new string[] { "cache", "redis" });
    }
}
