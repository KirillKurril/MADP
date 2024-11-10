using ALWD.Domain.Entities;
using ALWD.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ALWD.UI.Services.CategoryService;
using ALWD.UI.Services.ProductService;
using ALWD.Domain.Models;
using ALWD.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ALWD.Tests
{
	public class UICatalogControllerTests
	{
		[Fact]
		public async Task Index_InvalidCategoryListReceiving_Returns404()
		{
			var categoryService = Substitute.For<ICategoryService>();
			var productService = Substitute.For<IProductService>();

			categoryService.GetCategoryListAsync()
				.Returns(Task.FromResult<ResponseData<IReadOnlyList<Category>>>(null));

			var controller = new CatalogController(categoryService, productService);

			var result = await controller.Index(null);

			Assert.IsType<NotFoundObjectResult>(result);
		}

		[Fact]
		public async void Index_InvalidProductListReceiving_Return404()
		{
			var categoryService = Substitute.For<ICategoryService>();
			var productService = Substitute.For<IProductService>();

			categoryService.GetCategoryListAsync()
				.Returns(Task.FromResult(new ResponseData<IReadOnlyList<Category>>
											(new List<Category>()
												{ new Category(){ Id=1, Name="test", NormalizedName="test" } })));

			productService
				.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
				.Returns(new ResponseData<ListModel<Product>>(null, false, "ErrorMessage"));

			var controller = new CatalogController(categoryService, productService);

			var result = await controller.Index(null);

			Assert.IsType<NotFoundObjectResult>(result);
		}

		[Fact]
		public async void Index_CorrectDataReceiving_CorrectViewDataCategoryList()
		{
			var categoryService = Substitute.For<ICategoryService>();
			var productService = Substitute.For<IProductService>();

			List<Category> categories = new List<Category>()
				{ new Category(){ Id=1, Name="test", NormalizedName="test" } };
			var categoriesResponse = new ResponseData<IReadOnlyList<Category>>(categories);

			categoryService.GetCategoryListAsync()
				.Returns(Task.FromResult(categoriesResponse));

			productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
				.Returns(Task.FromResult
				( new ResponseData<ListModel<Product>>
				( new ListModel<Product>
				( new List<Product>() { new Product() { Id = 1, Name = "testProductName" } } ))));

			var controller = new CatalogController(categoryService, productService);
			controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};

			var result = await controller.Index(null);

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<CatalogViewModel>(viewResult.Model);

			Assert.Equal(categoriesResponse, model.Categories);
		}

		[Fact]
		public async void Index_CorrectDataReceiving_CorrectViewDataCurrentCategory()
		{
			var categoryService = Substitute.For<ICategoryService>();
			var productService = Substitute.For<IProductService>();

			var testCategory = new Category() { Id = 1, Name = "test", NormalizedName = "test" };
			List<Category> testCategoryList = new List<Category>()
				{testCategory};
			var categoriesResponse = new ResponseData<IReadOnlyList<Category>>(testCategoryList);

			categoryService.GetCategoryListAsync()
				.Returns(Task.FromResult(categoriesResponse));

			productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
				.Returns(Task.FromResult
				(new ResponseData<ListModel<Product>>
				(new ListModel<Product>
				(new List<Product>() { new Product() { Id = 1, Name = "testProductName" } }))));

			var controller = new CatalogController(categoryService, productService);
			controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};

			string testCategoryJson = JsonSerializer.Serialize(testCategory);

			var result = await controller.Index(testCategoryJson);

			var viewResult = Assert.IsType<ViewResult>(result);

			Assert.Equal(viewResult.ViewData["currentCategory"], testCategory.Name);
		}

		[Fact]
		public async void Index_CorrectDataReceiving_CorrectViewDataProductList()
		{
			var categoryService = Substitute.For<ICategoryService>();
			var productService = Substitute.For<IProductService>();

			List<Category> categories = new List<Category>()
				{ new Category(){ Id=1, Name="test", NormalizedName="test" } };

			Product testProduct = new Product() { Id = 1, Name = "testproduct" };
			List<Product> products = new List<Product>(){testProduct};

			var categoriesResponse = new ResponseData<IReadOnlyList<Category>>(categories);

			categoryService.GetCategoryListAsync()
				.Returns(Task.FromResult(categoriesResponse));

			productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
				.Returns(Task.FromResult
				(new ResponseData<ListModel<Product>>
				(new ListModel<Product>(products))));

			var controller = new CatalogController(categoryService, productService);
			controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};

			var result = await controller.Index(null);

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<CatalogViewModel>(viewResult.Model);

			Assert.Contains(testProduct, model.ProductResponse.Data.Items);
		}
	}
}