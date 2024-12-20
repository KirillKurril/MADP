﻿using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ALWD.Blazor.WebAssembly.Services.CategoryService
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
    }
}
