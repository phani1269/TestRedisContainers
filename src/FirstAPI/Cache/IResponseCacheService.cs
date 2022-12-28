namespace ProductAPI.Cache
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string CacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string CacheKey);
    }
}



