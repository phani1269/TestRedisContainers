using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ProductResponseCaching.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public ResponseCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task CacheResponseAsync(string CacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return ; 
            }
            var serializedResponse = JsonSerializer.Serialize(response);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(timeToLive);

            _memoryCache.Set(CacheKey, serializedResponse, cacheEntryOptions);
        }

        public object  GetCachedResponseAsync(string CacheKey)
        {
            var cahedResponse = _memoryCache.Get(CacheKey);
            var serializedResponse = JsonSerializer.Serialize(cahedResponse);

            if (cahedResponse == null)
            {
                return null;

            }
            return cahedResponse;
        }
    }
}
