using ALWD.UI.Models;
using ALWD.UI.Services.CategoryService;
using ALWD.UI.Services.ProductService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services);
        var app = builder.Build();

        ConfigureMiddleware(app);
        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddSingleton<UriData>();
        services.AddRazorPages();

        var apiUri = services.BuildServiceProvider().GetService<IConfiguration>().GetSection("UriData:ApiUri").Value;

        services.AddHttpClient<IProductService, ApiProductService>(opt =>
            opt.BaseAddress = new Uri(apiUri));

        services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
            opt.BaseAddress = new Uri(apiUri));
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
