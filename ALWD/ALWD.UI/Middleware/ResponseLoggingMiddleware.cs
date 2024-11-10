using Serilog;

namespace ALWD.UI.Middleware
{
	public class ResponseLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public ResponseLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var request = context.Request;
			var response = context.Response;
			var statusCode = response.StatusCode;

			if (statusCode / 100 != 2)
			{
				Log.Warning($"---> request {request.Path.ToString()} returns {statusCode}");
			}

			await _next(context);
		}
	}
}
