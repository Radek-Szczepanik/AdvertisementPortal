using System.Diagnostics;

namespace AdvertisementPortal.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> logger;
        private Stopwatch stopWatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            stopWatch = new Stopwatch();
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            stopWatch.Start();
            await next.Invoke(context);
            stopWatch.Stop();

            var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
            if (elapsedMilliseconds / 1000 > 4)
            {
                var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds} ms";

                logger.LogInformation(message);
            }
        }
    }
}
