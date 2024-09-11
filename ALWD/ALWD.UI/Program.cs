using ALWD.UI.Extensions;
using ALWD.UI.Models;
using ALWD.UI.Services.CategoryService;
using ALWD.UI.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<UriData>();
builder.RegisterCustomServices();

var apiUri = builder.Configuration.GetSection("UriData:ApiUri").Value;

builder.Services
	.AddHttpClient<IProductService, ApiProductService>(opt =>
	opt.BaseAddress = new Uri(apiUri));

builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri(apiUri));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

