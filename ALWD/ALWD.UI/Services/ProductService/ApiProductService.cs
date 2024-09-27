using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.Validation.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ALWD.Domain.DTOs;
using Microsoft.AspNetCore.Http;

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

        public async Task<ResponseData<int>> CreateProductAsync(Product product, IFormFile? formFile)
          {
              var uri = $"{_httpClient.BaseAddress.AbsoluteUri}Product";

              var multipartContent = new MultipartFormDataContent();

              string productJson = JsonConvert.SerializeObject(product);
              HttpContent productContent = new StringContent(productJson, Encoding.UTF8, "application/json");
              multipartContent.Add(productContent, "product");

              if (formFile != null)
              {
                  await using var ms = new MemoryStream();
                  await formFile.CopyToAsync(ms);
                  byte[] fileBytes = ms.ToArray();

                  ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
                  fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                  multipartContent.Add(fileContent, "file", formFile.FileName);
              }

              _logger.LogInformation($"Requesting URL: {uri}");
              HttpResponseMessage response = await _httpClient.PostAsync(uri, multipartContent);

              if (!response.IsSuccessStatusCode)
              {
                  _logger.LogError($"-----> Unable to create product. Error: {response.Content}");
                  throw new HttpRequestException($"Error creating product: {response.Content}");
              }

            int createdId;
            try
            {
                createdId = await response.Content.ReadFromJsonAsync<int>(_serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
                return new ResponseData<int>(-1, false, $"Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
            }
            return new ResponseData<int>(createdId);
        }

        public async Task<ResponseData<int>> CreateProductAsync(ProductCreateValidationModel model)
        {
            var uri = $"{_httpClient.BaseAddress.AbsoluteUri}Products";

            CreateProductDTO dto = new CreateProductDTO()
            {
                ProductName = model.Name,
                ProductDescription = model.Description,
                ProductPrice = model.Price,
                ProductQuantity = model.Quantity,
                ProductCategoryId = model.CategoryId,
                ImageName = model.Image.FileName,
                ImageMimeType = model.Image.ContentType,
            };

            if (model.Image != null)
            {
                await using var ms = new MemoryStream();
                await model.Image.CopyToAsync(ms);
                dto.ImageContent = ms.ToArray();
            }

            //string json = JsonConvert.SerializeObject(dto);
            //HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //_logger.LogError(uri.ToString());
            //HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
            

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, dto, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to create product. Error: {response.Content}");
                throw new HttpRequestException($"Error creating product: {response.Content}");
            }

            int createdId;
            try
            {
                createdId  = await response.Content.ReadFromJsonAsync<int>(_serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
                return new ResponseData<int>(-1, false, $"Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
            }
            return new ResponseData<int>(createdId);
        }

        public async Task<ResponseData<int>> UpdateProductAsync(ProductEditValidationModel model)
        {
            var uri = $"{_httpClient.BaseAddress.AbsoluteUri}Products";

            UpdateProductDTO dto = new UpdateProductDTO()
            {
                ProductName = model.Name,
                ProductDescription = model.Description,
                ProductPrice = model.Price,
                ProductQuantity = model.Quantity,
                ProductCategoryId = model.CategoryId,
                ImageName = model.Image.FileName,
                ImageMimeType = model.Image.ContentType,
                ProductId = model.Id,
            };

            if (model.Image != null)
            {
                await using var ms = new MemoryStream();
                await model.Image.CopyToAsync(ms);
                dto.ImageContent = ms.ToArray();
            }

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, dto, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to put product. Error: {response.Content}, Code: {response.StatusCode}");
                throw new HttpRequestException($"Error creating product: {response.Content}, Code: {response.StatusCode}");
            }

            int createdId;
            try
            {
                createdId = await response.Content.ReadFromJsonAsync<int>(_serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
                return new ResponseData<int>(-1, false, $"Reading JSON failure. ApiProductService/GetProductListAsync(): {ex.Message}");
            }
            return new ResponseData<int>(createdId);
        }
        
        public async Task<ResponseData<bool>> DeleteProductAsync(int id)
        {
            var uri = $"{_httpClient.BaseAddress.AbsoluteUri}Products/{id}";

            HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Unable to delete product with id {id}. Error: {response.Content}, Code: {response.StatusCode}");
                throw new HttpRequestException($"Unable to delete product with id {id}. Error: {response.Content}, Code: {response.StatusCode}");
            }
            return new ResponseData<bool>(true);
        }

        public Task<ResponseData<int>> UpdateProductAsync(Product product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
