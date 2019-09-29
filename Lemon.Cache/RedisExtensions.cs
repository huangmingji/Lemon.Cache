using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.Cache
{
    /// <summary>
    /// redis扩展
    /// </summary>
    public static class RedisExtensions
    {
        /// <summary>
        /// 注入Redis分布式缓存服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheDI(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connections = configuration.GetSection("RedisConfig:RedisConnections").Value.Split(',').ToList();
            string instanceName = configuration.GetSection("RedisConfig:InstanceName").Value;
            string password = configuration.GetSection("RedisConfig:Password").Value;
            int asyncTimeout = Int32.Parse(configuration.GetSection("RedisConfig:AsyncTimeout")?.Value??"5000");
            int syncTimeout = Int32.Parse(configuration.GetSection("RedisConfig:SyncTimeout")?.Value??"5000");
            int connectTimeout = Int32.Parse(configuration.GetSection("RedisConfig:ConnectTimeout")?.Value??"5000");
            int defaultDatabase = Int32.Parse(configuration.GetSection("RedisConfig:DefaultDatabase")?.Value??"1");

            connections.RemoveAll(x => string.IsNullOrWhiteSpace(x));

            if (connections.Count == 0)
            {
                throw new Exception("RedisConnections配置错误");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions();
                if (!string.IsNullOrWhiteSpace(instanceName))
                {
                    options.InstanceName = string.Format("{0}:", instanceName);
                }
                if (!string.IsNullOrWhiteSpace(password))
                {
                    options.ConfigurationOptions.Password = configuration.GetSection("RedisConfig:Password").Value;
                }
                if (asyncTimeout > 0)
                {
                    options.ConfigurationOptions.AsyncTimeout = asyncTimeout;
                }
                if (connectTimeout > 0)
                {
                    options.ConfigurationOptions.SyncTimeout = syncTimeout;
                }
                if (connectTimeout > 0)
                {
                    options.ConfigurationOptions.ConnectTimeout = connectTimeout;
                }
                if (defaultDatabase >= 0 && defaultDatabase <= 15)
                {
                    options.ConfigurationOptions.DefaultDatabase = defaultDatabase;
                }
                foreach (string connection in connections)
                {
                    if (! string.IsNullOrWhiteSpace(connection))
                    {
                        options.ConfigurationOptions.EndPoints.Add(connection);
                    }
                }
            });
            return services;
        }
    }
}
