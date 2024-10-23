namespace ALWD.API.Extensions
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using System.IO;
	using System.Threading.Tasks;

	public class RequestLogger
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<RequestLogger> _logger;

		public RequestLogger(RequestDelegate next, ILogger<RequestLogger> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			_logger.LogInformation("\n!\n!\n!\n!\nReceived HTTP request: {Method} {Path}",
				context.Request.Method,
				context.Request.Path);

			context.Request.EnableBuffering();

			var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
			if (!string.IsNullOrEmpty(bodyAsText))
			{
				_logger.LogInformation("Request Body: {Body}", bodyAsText);
				context.Request.Body.Position = 0;
			}

			await _next(context);
		}
	}

}
