using CacheLibrary.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CacheLibrary
{
    public static class CacheStartupRegistration
    {
        public static IServiceCollection AddCacheRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var deltaBackOffms = Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds);
            var maxdeltaBackOffms = Convert.ToInt32(TimeSpan.FromSeconds(20).TotalMilliseconds);

            var options = new ConfigurationOptions
            {

                EndPoints = { $"{configuration.GetValue<string>("RedisCache:Host")}:{configuration.GetValue<int>("RedisCache:Port")}" },
                ConnectRetry = 5,
                ReconnectRetryPolicy = new ExponentialRetry(deltaBackOffms,
                                                maxdeltaBackOffms),
                ConnectTimeout = 1000,
                AbortOnConnectFail = false,
                SyncTimeout = 10000
            };



            var redisMultiplexer = ConnectionMultiplexer.Connect(options);
            //var test = ConnectionMultiplexer.Connect(configuration.)

            services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);
            services.AddScoped<ICacheService, CacheService>();


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{configuration.GetValue<string>("RedisCache:Host")}:{configuration.GetValue<int>("RedisCache:Port")}";
            });

            return services;
        }
    }
}
