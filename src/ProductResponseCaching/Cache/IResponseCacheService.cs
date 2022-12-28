namespace ProductResponseCaching.Cache
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string CacheKey, object response, TimeSpan timeToLive);
        object GetCachedResponseAsync(string CacheKey);
    }
}
