using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ProductResponseCaching.Cache
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse =  cacheService.GetCachedResponseAsync(cacheKey);
            if (cachedResponse!=null)
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse.ToString(),
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            if (executedContext.Result is OkObjectResult objectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));

            }

        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilders = new StringBuilder();

            keyBuilders.Append($"{request.Path}");

            foreach (var (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                keyBuilders.Append($"|{key}-{value}");
            }
            return keyBuilders.ToString();
        }
    }
}
