using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
				ReferenceHandler = ReferenceHandler.Preserve,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Products";

            Dictionary<string, string> parameters = new();

            parameters.Add("itemsPerPage", _itemsPerPage);

            if (categoryNormalizedName != null)
                parameters.Add("category", categoryNormalizedName);

            parameters.Add("page", pageNo.ToString());
            string urlWithQuery = QueryHelpers.AddQueryString(baseUri, parameters);

            var response = await _httpClient.GetAsync(
            new Uri(urlWithQuery));

            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.
                            Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions);
                }
                catch (System.Text.Json.JsonException ex)
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
        public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Products/{id}");

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogError($"-----> Error while parsing JSON: {ex.Message}");
                    return new ResponseData<Product>(null, false, $"JSON Parsing Error: {ex.Message}");
                }
            }

            _logger.LogError($"-----> Product not found. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Product>(null, false, $"Product not found. Error: {response.StatusCode.ToString()}");
        }

        public async Task CreateProductAsync(Product product, IFormFile? formFile)
        {
            var uri = new Uri($"{_httpClient.BaseAddress}Products");

            var multipartContent = new MultipartFormDataContent();

            var productJson = JsonConvert.SerializeObject(product);
            var productContent = new StringContent(productJson, Encoding.UTF8, "application/json");
            multipartContent.Add(productContent, "product");

            if (formFile != null)
            {
                await using var ms = new MemoryStream();
                await formFile.CopyToAsync(ms);
                var fileBytes = ms.ToArray();

                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                multipartContent.Add(fileContent, "formFile", formFile.FileName);
            }

            HttpResponseMessage response = await _httpClient.PostAsync(uri, multipartContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to create product. Error: {response.StatusCode}");
                throw new HttpRequestException($"Error creating product: {response.StatusCode}");
            }
        }

        public async Task UpdateProductAsync(Product product, IFormFile? formFile)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Productes/{product.Id}");

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
        public async Task DeleteProductAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Productes/{id}");

            var response = await _httpClient.DeleteAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to delete product. Error: {response.StatusCode.ToString()}");
                throw new HttpRequestException($"Error deletion product: {response.StatusCode}");
            }
        }
    }
}
