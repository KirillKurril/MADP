using ALWD.API.Middleware;

namespace ALWD.API.Extensions
{
	public static class ResponseLoggingExtension
	{
		public static IApplicationBuilder UseResponseLoggingMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ResponseLoggingMiddleware>();
		}
	}

}
