using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ProductAPI.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string CacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return ; 
            }
            var serializedResponse = JsonSerializer.Serialize(response);
            
            await _distributedCache.SetStringAsync(CacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string> GetCachedResponseAsync(string CacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(CacheKey);
            return String.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}
