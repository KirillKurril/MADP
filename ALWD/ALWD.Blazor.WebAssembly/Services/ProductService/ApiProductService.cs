using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.Services.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using JsonException = System.Text.Json.JsonException;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http;

namespace ALWD.Blazor.WebAssembly.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        string _itemsPerPage;
        HttpClient _httpClient;
        IAccessTokenProvider _tokenProvider;
        JsonSerializerOptions _serializerOptions;
        ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiProductService> logger,
            IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _itemsPerPage = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1)
        {
            var baseUri = $"{_httpClient.BaseAddress.AbsoluteUri}Products";

            Dictionary<string, string> parameters = new();

            ;
            parameters.Add("itemsPerPage", _itemsPerPage);

            if (categoryNormalizedName != null)
                parameters.Add("category", categoryNormalizedName);

            parameters.Add("page", pageNo.ToString());
            string urlWithQuery = QueryHelpers.AddQueryString(baseUri, parameters);


            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                var accessToken = token.Value;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken); ;
            }
            else
            {
                throw new Exception("Token obtaining Failure");
            }

            var response = await _httpClient.GetAsync(new Uri(urlWithQuery));

            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"-----> Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
                    return new ResponseData<ListModel<Product>>(null, false, $"Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Product>>(null, false, $"Data not received from server {response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Products/{id}");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                var accessToken = token.Value;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken); ;
            }
            else
            {
                throw new Exception("Token obtaining Failure");
            }

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

    }
}
