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

        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories";

            var response = await _httpClient.PostAsJsonAsync(baseUri, category, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Category>>();
                return data;
            }

            _logger.LogError($"-----> Category not created. Error: {response.StatusCode}");

            return new ResponseData<Category>(null, false, $"Category not created. Error: {response.StatusCode}");
        }

        public async Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories/{id}";

            var response = await _httpClient.PutAsJsonAsync(baseUri, category, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Category>>();
                return data;
            }

            _logger.LogError($"-----> Category with ID {id} not updated. Error: {response.StatusCode}");

            return new ResponseData<Category>(null, false, $"Category with ID {id} not updated. Error: {response.StatusCode}");
        }

        public async Task<ResponseData<bool>> DeleteCategoryAsync(int id)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}/Categories/{id}";

            var response = await _httpClient.DeleteAsync(baseUri);

            if (response.IsSuccessStatusCode)
            {
                return new ResponseData<bool>(true, true, null); // Успешное удаление
            }

            _logger.LogError($"-----> Category with ID {id} not deleted. Error: {response.StatusCode}");

            return new ResponseData<bool>(false, false, $"Category with ID {id} not deleted. Error: {response.StatusCode}");
        }
    }
}
