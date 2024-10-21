using ALWD.API.Models;
using ALWD.UI.Models;
using ALWD.UI.Services.Authentication;
using ALWD.UI.Services.CategoryService;
using ALWD.UI.Services.ProductService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ALWD.Domain.Services.Authentication;
using ALWD.Domain.Models;
using ALWD.UI.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        var app = builder.Build();

        ConfigureMiddleware(app);
        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
		builder.Services.ConfigureKeycloak(builder.Configuration);

		builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<UriData>();
		builder.Services.AddRazorPages();
        builder.Services.AddScoped<ITokenAccessor, KeycloakTokenAccessor>();
        builder.Services.AddScoped<IAuthService, KeycloakAuthService>();

        var apiUri = builder.Services
			.BuildServiceProvider()!
            .GetService<IConfiguration>()!
            .GetSection("UriData:ApiUri")
            .Value;



		builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
            opt.BaseAddress = new Uri(apiUri));

		builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
	        opt.BaseAddress = new Uri(apiUri));



		var keycloakData =
		builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
		builder.Services
		.AddAuthentication(options =>
		{
			options.DefaultScheme =	CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
		})
		.AddCookie()
		.AddJwtBearer()
		.AddOpenIdConnect(options =>
		{
			options.Authority =
	            $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
			
            options.ClientId = keycloakData.ClientId;
			options.ClientSecret = keycloakData.ClientSecret;
			options.ResponseType = OpenIdConnectResponseType.Code;
			options.Scope.Add("openid");
			options.SaveTokens = true;
			options.RequireHttpsMetadata = false; 
		    options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
		    });

		builder.Services.AddAuthorization(opt =>
		{
			opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
		});

	}

	private static void ConfigureMiddleware(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        app.MapRazorPages();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapGet("/Admin", async context =>
        {
            context.Response.Redirect("/Admin/Index");
        });
    }
}
