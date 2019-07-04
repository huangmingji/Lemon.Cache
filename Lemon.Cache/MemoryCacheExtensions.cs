using System;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.Cache
{
    public static class MemoryCacheExtensions
    {
        public static IServiceCollection AddMemoryCacheDI(this IServiceCollection services)
        {
            return services.AddMemoryCache();
        }
    }
}
