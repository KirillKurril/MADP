using System.Net.Http;
using System.Text.Json;
using System.Text;
using ALWD.API.Services.FileService;
using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.Domain.Services.Authentication;
using ALWD.Domain.Models;

namespace ALWD.API.Services.AccountService
{
    public class KeycloakAccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;

        public KeycloakAccountService(HttpClient httpClient, ITokenAccessor tokenService)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenService;
        }
        public async Task<ResponseData<bool>> UpdateAvatar(string userUri, string newAvatarUri)
        {
            var updatedUser = new
            {
                attributes = new Dictionary<string, object>
                    {
                        { "AvatarUri", newAvatarUri } 
                    }
            };

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var userData = JsonSerializer.Serialize(updatedUser, serializerOptions);
            HttpContent content = new StringContent(userData, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PutAsync(userUri, content);
            }
            catch (Exception ex)
            {
                return new ResponseData<bool>(false, false, $"class: ProductService, method: CreateProductAsync: removing product: {ex.Message}");
            }

            if(!response.IsSuccessStatusCode)
                return new ResponseData<bool>(false, false, response.StatusCode.ToString() + " " + response.Content);

            return new ResponseData<bool>(true);

        }

    }
}
