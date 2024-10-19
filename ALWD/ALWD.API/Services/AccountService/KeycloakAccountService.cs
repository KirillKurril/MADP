using System.Net.Http;
using System.Text.Json;
using System.Text;
using ALWD.API.Services.FileService;
using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;

namespace ALWD.API.Services.AccountService
{
    public class KeycloakAccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public KeycloakAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> UpdateUserAvatarIdAsync(string userUri, string newAvatarId)
        {
            var updatedUser = new
            {
                attributes = new Dictionary<string, object>
                    {
                        { "avatarId", newAvatarId } 
                    }
            };

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var userData = JsonSerializer.Serialize(updatedUser, serializerOptions);
            HttpContent content = new StringContent(userData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(userUri, content);

            return response.IsSuccessStatusCode;
        }

    }
}
