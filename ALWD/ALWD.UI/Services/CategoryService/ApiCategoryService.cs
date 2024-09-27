using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.Validation.Models;
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

        public async Task<ResponseData<int>> CreateCategoryAsync(CategoryCreateValidationModel model)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories";

            Category category = new Category()
            {
                Name = model.Name,
                NormalizedName = model.NormalizedName,
            };

            var response = await _httpClient.PostAsJsonAsync(baseUri, category, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to create category. Error: {response.StatusCode.ToString()}");
                throw new HttpRequestException($"Error creating category: {response.StatusCode}");
            }

            int createdId;
            try
            {
                createdId = await response.Content.ReadFromJsonAsync<int>(_serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Reading JSON failure. ApiCategoryService/CreateCategoryAsync(): {ex.Message}");
                return new ResponseData<int>(-1, false, $"Reading JSON failure. ApiCategoryService/CreateCategoryAsync(): {ex.Message}");
            }
            return new ResponseData<int>(createdId);
        }

        public async Task<ResponseData<int>> UpdateCategoryAsync(CategoryEditValidationModel model)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories/{model.Id}";

            Category category = new Category()
            {
                Id = model.Id,
                Name = model.Name,
                NormalizedName = model.NormalizedName,
            };

            var response = await _httpClient.PutAsJsonAsync(baseUri, category, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
				var errorMessage = await response.Content.ReadAsStringAsync();

				_logger.LogError($"-----> Unable to update category. Error: {response.StatusCode.ToString()} | {errorMessage}");
                throw new HttpRequestException($"Error updating category: {response.StatusCode} | {errorMessage}");
            }

            int updatedId;
            try
            {
                updatedId = await response.Content.ReadFromJsonAsync<int>(_serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Reading JSON failure. ApiCategoryService/UpdateCategoryAsync(): {ex.Message}");
                return new ResponseData<int>(-1, false, $"Reading JSON failure. ApiCategoryService/UpdateCategoryAsync(): {ex.Message}");
            }
            return new ResponseData<int>(updatedId);

        }

        public async Task DeleteCategoryAsync(int id)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Categories/{id}";

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
