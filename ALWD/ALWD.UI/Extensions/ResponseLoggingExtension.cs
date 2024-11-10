using ALWD.UI.Middleware;

namespace ALWD.UI.Extensions
{
	public static class ResponseLoggingExtension
	{
		public static IApplicationBuilder UseResponseLoggingMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ResponseLoggingMiddleware>();
		}
	}

}
