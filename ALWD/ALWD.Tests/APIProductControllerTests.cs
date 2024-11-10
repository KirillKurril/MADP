using System.Linq.Expressions;
using ALWD.API.Services.FileService;
using ALWD.API.Services.ProductService;
using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ALWD.Tests
{
	public class APIProductServiceTests
	{
		int MAX_PAGE_SIZE = 3;
		IRepository<Product> _repository;
		IConfiguration _configuration;
		IWebHostEnvironment _hostEnv;
		IFileService _fileService;
		List<Product> _products;
		List<Category> _categories;
		public APIProductServiceTests()
        {
			_repository = Substitute.For<IRepository<Product>>();
			_configuration = Substitute.For<IConfiguration>();
			_hostEnv = Substitute.For<IWebHostEnvironment>();
			_fileService = Substitute.For<IFileService>();

			_configuration["MaxPageSize"].Returns(MAX_PAGE_SIZE.ToString());

			_categories = new List<Category>()
			{
				new Category() {Id=1, Name="testCategory1", NormalizedName="normalized1" },
				new Category() {Id=2, Name="testCategory2", NormalizedName="normalized2" }
			};

			_products = new List<Product>()
			{
				new Product() {Id=1,Name="testproduct1", Category=_categories[0]},
				new Product() {Id=2,Name="testproduct2", Category=_categories[1]},
				new Product() {Id=3,Name="testproduct3", Category=_categories[0]},
				new Product() {Id=4,Name="testproduct4", Category=_categories[1]},
				new Product() {Id=5,Name="testproduct5", Category=_categories[0]}
			};
		}

        [Fact]
		public async void GetProductsListAsync_DefaultRequest_ReturnCorrectProductsCountAndTotalPagesCount()
		{
			_repository.ListAsync(
					Arg.Any<Expression<Func<Product, bool>>>(),
					Arg.Any<CancellationToken>(),
					Arg.Any<Expression<Func<Product, object>>>(),
					Arg.Any<Expression<Func<Product, object>>>())
					.Returns(callInfo => Task.FromResult((IReadOnlyList<Product>)_products));


			var service = new ProductService(_repository, _configuration, _hostEnv, _fileService);
			var productListResponse = await service.GetProductsAsync(3, null, null);

			Assert.Equal(2, productListResponse.Data.TotalPages);
			Assert.Equal(5, productListResponse.Data.Items.Count);
		}

		[Fact]
		public async void GetProductsListAsync_CorrectRequestedPageNumber_ReturnCorrectProductList()
		{
			var expectedProductList = _products.GetRange(3, 2);

			_repository.ListAsync(
					Arg.Any<Expression<Func<Product, bool>>>(),
					Arg.Any<CancellationToken>(),
					Arg.Any<Expression<Func<Product, object>>>(),
					Arg.Any<Expression<Func<Product, object>>>())
					.Returns(callInfo => Task.FromResult((IReadOnlyList<Product>)_products));


			var service = new ProductService(_repository, _configuration, _hostEnv, _fileService);
			var productListResponse = await service.GetProductsAsync(3, null, 2);

			for (int i = 0; i < productListResponse.Data.Items.Count; ++i)
			{
				Assert.Equal(productListResponse.Data.Items[i].Id, expectedProductList[i].Id);
				Assert.Equal(productListResponse.Data.Items[i].Name, expectedProductList[i].Name);
			}
		}

		[Fact]
		public async void GetProductsListAsync_CorrectRequestedCategory_ReturnCorrectProductList()
		{
			List<Product> expectedProductList = _products.Where(p => p.Category.NormalizedName == "normalized1").ToList();

			_repository.ListAsync(
					Arg.Any<Expression<Func<Product, bool>>>(),
					Arg.Any<CancellationToken>(),
					Arg.Any<Expression<Func<Product, object>>>(),
					Arg.Any<Expression<Func<Product, object>>>())
					.Returns(callInfo => Task.FromResult((IReadOnlyList<Product>)_products.Where(p => p.Category.NormalizedName == "normalized1").ToList()));

			var service = new ProductService(_repository, _configuration, _hostEnv, _fileService);
			var productListResponse = await service.GetProductsAsync(3, "normalized1", null);

			for (int i = 0; i < productListResponse.Data.Items.Count; ++i)
			{
				Assert.Equal(productListResponse.Data.Items[i].Id, expectedProductList[i].Id);
				Assert.Equal(productListResponse.Data.Items[i].Name, expectedProductList[i].Name);
			}
		}

		[Fact]
		public async void GetProductsListAsync_IncorrectRequestedPageNumber_ReturnsDefaultNoncategoryProductList()
		{
			var expectedProductList = _products.GetRange(3, 2);

			_repository.ListAsync(
					Arg.Any<Expression<Func<Product, bool>>>(),
					Arg.Any<CancellationToken>(),
					Arg.Any<Expression<Func<Product, object>>>(),
					Arg.Any<Expression<Func<Product, object>>>())
					.Returns(callInfo => Task.FromResult((IReadOnlyList<Product>)_products));


			var service = new ProductService(_repository, _configuration, _hostEnv, _fileService);
			var productListResponse = await service.GetProductsAsync(3, null, 22);

			Assert.Equal("Incorrect page number", productListResponse.ErrorMessage);
		}

		[Fact]
		public async void GetProductListAsync_ExceedingItemsPerPageNumber_ReturnResponseSuccessfullStatusFalse()
		{
			var expectedProductList = _products.GetRange(0, 3);

			_repository.ListAsync(
					Arg.Any<Expression<Func<Product, bool>>>(),
					Arg.Any<CancellationToken>(),
					Arg.Any<Expression<Func<Product, object>>>(),
					Arg.Any<Expression<Func<Product, object>>>())
					.Returns(callInfo => Task.FromResult((IReadOnlyList<Product>)_products));


			var service = new ProductService(_repository, _configuration, _hostEnv, _fileService);
			var productListResponse = await service.GetProductsAsync(100, null, null);

			for (int i = 0; i < MAX_PAGE_SIZE; ++i)
			{
				Assert.Equal(productListResponse.Data.Items[i].Id, expectedProductList[i].Id);
				Assert.Equal(productListResponse.Data.Items[i].Name, expectedProductList[i].Name);
			}
		}
	}
}
