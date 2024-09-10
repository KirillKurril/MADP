using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ALWD.UI.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        string _itemsPerPage;
        HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _itemsPerPage = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Productes/");

            if (!_itemsPerPage.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _itemsPerPage));
            }

            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            }

            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            };


            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.
                            Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    var uiResponse = new ResponseData<ListModel<Product>>(null);
                    uiResponse.ErrorMessage = $"Error: {ex.Message}";
                    return uiResponse;
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Product>>(null, false, $"Data not received from server {response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Productes");

            var response = await _httpClient.PostAsJsonAsync(
            uri,
            product,
            _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object not created. Error: {response.StatusCode.ToString()}");

            return new ResponseData<Product>(null, false, $"Объект не добавлен. Error: {response.StatusCode.ToString()}");
        }
        public async Task DeleteProductAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Productes/{id}");

            var response = await _httpClient.DeleteAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to delete product. Error: {response.StatusCode.ToString()}");
                throw new HttpRequestException($"Error deleting product: {response.StatusCode}");
            }
        }

        public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Productes/{id}");

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Error while parsing JSON: {ex.Message}");
                    return new ResponseData<Product>(null, false, $"JSON Parsing Error: {ex.Message}");
                }
            }

            _logger.LogError($"-----> Product not found. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Product>(null, false, $"Product not found. Error: {response.StatusCode.ToString()}");
        }

        public async Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Productes/{id}");

            MultipartFormDataContent multipartContent = new MultipartFormDataContent();

            var productContent = JsonContent.Create(product, options: _serializerOptions);
            multipartContent.Add(productContent, "product");

            if (formFile != null)
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await formFile.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(formFile.ContentType);
                multipartContent.Add(fileContent, "formFile", formFile.FileName);
            }

            var response = await _httpClient.PutAsync(uri, multipartContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to update product. Error: {response.StatusCode.ToString()}");
                throw new HttpRequestException($"Error updating product: {response.StatusCode}");
            }
        }
    }
}
