using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.ProductService;
using System.Text.Json;

namespace ALWD.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        ILogger<ApiCategoryService> _logger;
        public ApiCategoryService(HttpClient httpClient,
            ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            _logger = logger;
        }
        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Categories/{id}");

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Category>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Error while parsing JSON: {ex.Message}");
                    return new ResponseData<Category>(null, false, $"JSON Parsing Error: {ex.Message}");
                }
            }

            _logger.LogError($"-----> Product not found. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Category>(null, false, $"Category not found. Error: {response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync()
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories";
            var response = await _httpClient.GetAsync(new Uri(baseUri));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var categoryResponseData = await response.Content.ReadFromJsonAsync<ResponseData<IReadOnlyList<Category>>>(_serializerOptions);
                    return categoryResponseData;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    var uiResponse = new ResponseData<IReadOnlyList<Category>>(null);
                    uiResponse.ErrorMessage = $"Error: {ex.Message}";
                    return uiResponse;
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<IReadOnlyList<Category>>(null, false, $"Data not received from server {response.StatusCode.ToString()}");
        }

        public async Task CreateCategoryAsync(Category category)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories";

            var response = await _httpClient.PostAsJsonAsync(baseUri, category, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to create category. Error: {response.StatusCode.ToString()}");
                throw new HttpRequestException($"Error creating category: {response.StatusCode}");
            }

            _logger.LogError($"-----> Category created. successfilly: {response.StatusCode}");
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories/{category.Id}";

            var response = await _httpClient.PutAsJsonAsync(baseUri, category, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to update category. Error: {response.StatusCode.ToString()}");
                throw new HttpRequestException($"Error updating category: {response.StatusCode}");
            }

            _logger.LogError($"-----> Category with ID updateded successfully: {response.StatusCode}");

        }

        public async Task DeleteCategoryAsync(int id)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}/Categories/{id}";

            var response = await _httpClient.DeleteAsync(baseUri);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to delete category. Error: {response.StatusCode.ToString()}");
                throw new HttpRequestException($"Error deleting category: {response.StatusCode}");
            }

            _logger.LogError($"-----> Category with ID {id} deleted successfully. Error: {response.StatusCode}");

        }
    }
}
