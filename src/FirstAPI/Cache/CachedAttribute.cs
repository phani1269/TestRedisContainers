using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ProductAPI.Cache
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

            if (context.HttpContext.Request.Headers.CacheControl == "public")
            {

                var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);


                if (!string.IsNullOrEmpty(cachedResponse))
                {
                    var contentResult = new ContentResult
                    {
                        Content = cachedResponse,
                        ContentType = "application/json",
                        StatusCode = 200,

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
            else
            {
                await next();
            };

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
