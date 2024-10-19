using ALWD.API.Services.FileService;
using ALWD.UI.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using ALWD.Domain.Models;
using ALWD.UI.Services.ProductService;
using ALWD.Domain.DTOs;
using ALWD.Domain.Services.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ALWD.UI.Services.Authentication
{
    public class KeycloakAuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;
        ILogger<ApiProductService> _logger;
        KeycloakData _keycloakData;
        public KeycloakAuthService(HttpClient httpClient,
        IOptions<KeycloakData> options,
        ITokenAccessor tokenAccessor,
        ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
            _keycloakData = options.Value;
            _logger = logger;
        }
        public async Task<ResponseData<bool>> RegisterUserAsync(string email,
                                                                string password,
                                                                string? name, 
                                                                IFormFile? image)
        {
            try
            {
                await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            }
            catch (Exception ex)
            {
                return new ResponseData<bool>(false, false, ex.Message);
            }


            CreateUserModel newUser = new();
            newUser.Attributes.Add("avatarId", "1");
            newUser.Email = email;
            newUser.Username = name ?? email;
            newUser.Credentials.Add(new UserCredentials { Value = password });
            
            var requestUri = $"{_keycloakData.Host}/admin/realms/{_keycloakData.Realm}/users";
            
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var userData = JsonSerializer.Serialize(newUser, serializerOptions);
            HttpContent content = new StringContent(userData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
                return new ResponseData<bool>(false, false, response.StatusCode + " " + response.Content);

            string userUri = response.Headers.Location!.AbsoluteUri;


            if (image != null)
            {
                using (var ms = new MemoryStream()) {
                    image.CopyTo(ms);

                    UploadImageDTO dto = new()
                    {
                        ImageContent = ms.ToArray(),
                        ImageMimeType = image.ContentType,
                        ImageName = image.Name,
                        UserUri = userUri
                    };
                }
            }

            return new ResponseData<bool>(true);
        }
    }

}
