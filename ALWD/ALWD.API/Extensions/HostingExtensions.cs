using ALWD.Domain.Models;

namespace ALWD.API.Extensions
{
		public static class HostingExtensions
		{
			public static IServiceCollection ConfigureKeycloak(this IServiceCollection services, IConfiguration configuration)
			{
				services.Configure<KeycloakData>(configuration.GetSection("Keycloak"));
				return services;
			}
		}
}
