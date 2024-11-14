using ALWD.Blazor.WebAssembly.Services.CategoryService;
using ALWD.Blazor.WebAssembly.Services.ProductService;
using ALWD.Domain.Services.Authentication;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ALWD.Blazor.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebAssemblyHostBuilder.CreateDefault(args);

                ConfigureRootComponents(builder);
                ConfigureServices(builder);

                await RunAppAsync(builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void ConfigureRootComponents(WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
        }

        private static void ConfigureServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<ITokenAccessor, KeycloakTokenAccessor>();

            var apiUri = builder.Services
			    .BuildServiceProvider()!
                .GetService<IConfiguration>()!
                .GetSection("UriData:ApiUri")
                .Value;



		    builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
                opt.BaseAddress = new Uri(apiUri));

		    builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
	            opt.BaseAddress = new Uri(apiUri));

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Keycloak", options.ProviderOptions);
            });
        }

        private static async Task RunAppAsync(WebAssemblyHostBuilder builder)
        {
            var host = builder.Build();
            await host.RunAsync();
        }
    }
}

