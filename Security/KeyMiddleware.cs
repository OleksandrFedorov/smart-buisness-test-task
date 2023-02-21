namespace TestTask.Security
{
    public class KeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _key = "Key";

        public KeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_key, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("No API key was provided.");
                return;
            }

            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>(_key) ?? string.Empty;

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API key.");
                return;
            }

            await _next(context);
        }
    }
}
