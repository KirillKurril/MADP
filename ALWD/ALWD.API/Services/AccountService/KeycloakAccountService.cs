using System.Text.Json;
using System.Text;
using ALWD.Domain.Services.Authentication;
using ALWD.Domain.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace ALWD.API.Services.AccountService
{
    public class KeycloakAccountService : IAccountService
    {
		private readonly HttpClient _httpClient;
		private readonly ITokenAccessor _tokenAccessor;
		private readonly IHttpContextAccessor _httpContextAccessor;
		KeycloakData _keycloakData;


		public KeycloakAccountService(HttpClient httpClient,
			IOptions<KeycloakData> options,
			ITokenAccessor tokenAccessor,
			IHttpContextAccessor httpContextAccessor)
		{
			_httpClient = httpClient;
			_httpContextAccessor = httpContextAccessor;
			_tokenAccessor = tokenAccessor;
			_keycloakData = options.Value;
		}
		public async Task<ResponseData<bool>> UpdateAvatar(string userUri, string accessToken, string imageUri)
        {
            var updatedUser = new
            {
                attributes = new Dictionary<string, object>
                    {
                        { "avatar", imageUri } 
                    }
            };

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			string jsonContent = JsonSerializer.Serialize(updatedUser, serializerOptions);
			HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(userUri, content);

			if (response.IsSuccessStatusCode)
			{
				return new ResponseData<bool>(true);
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				return new ResponseData<bool>(false, false, $"Error updating avatar: {response.ReasonPhrase} - {errorContent}");
			}
		}

    }
}
