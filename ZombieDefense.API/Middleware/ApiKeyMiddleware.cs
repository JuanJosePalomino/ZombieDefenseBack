namespace ZombieDefense.API.Middleware {
    public class ApiKeyMiddleware {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private const string ApiKeyHeaderName = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration) {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context) {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey)) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key faltante.");
                return;
            }

            var apiKeyConfig = _configuration["Security:ApiKey"];
            if (apiKeyConfig is null || extractedApiKey != apiKeyConfig) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key inválida");
                return;
            }

            await _next(context);
        }
    }
}
