using System.Text.Json;

namespace UserandDocumentManagement_JKT.CustomMiddileWare
{
    public class GlobalResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }
       public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await _next(context);

            newBody.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(newBody).ReadToEndAsync();
            newBody.Seek(0, SeekOrigin.Begin);

            object? responseData = null;
            bool isJson = context.Response.ContentType != null &&
                          context.Response.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase);

            if (isJson && !string.IsNullOrWhiteSpace(responseBody))
            {
                try
                {
                    responseData = JsonSerializer.Deserialize<object>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch
                {
                    // If deserialization fails, treat as raw string
                    responseData = responseBody;
                }
            }
            else
            {
                responseData = responseBody;
            }

            var statusCode = context.Response.StatusCode;
            var customResponse = new
            {
                DateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                Data = statusCode >= 200 && statusCode < 300 ? responseData : null,
                Error = statusCode >= 400 ? responseData : null,
                Status = statusCode
            };

            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(customResponse));
        }

    }
}
