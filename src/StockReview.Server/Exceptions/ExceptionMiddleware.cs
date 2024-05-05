using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StockReview.Server.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("触发了异常, 但是Response HasStarted!");
                    throw;
                }

                await HandlerException(context, e);
            }
        }

        private async Task HandlerException(HttpContext httpContext, Exception exception)
        {
            var errorData = new ApiResponse(1, exception.Message, null);
            var errorResponse = JsonConvert.SerializeObject(errorData, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
            );
            httpContext.Items["nl-items-errorCode"] = "error";
            httpContext.Items["nl-items-middleware"] = "ExceptionMiddleware";
            _logger.LogError(exception, errorResponse);
            // 这里强写200
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(errorResponse);
        }

    }
}