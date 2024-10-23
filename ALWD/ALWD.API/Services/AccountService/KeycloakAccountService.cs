using System.Text.Json;
using System.Text;
using ALWD.Domain.Services.Authentication;
using ALWD.Domain.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace ALWD.API.Services.AccountService
{
    public class KeycloakAccountService : IAccountService
    {
		private readonly HttpClient _httpClient;
		private readonly ITokenAccessor _tokenAccessor;
		private readonly IHttpContextAccessor _httpContextAccessor;
		JsonSerializerOptions _serializerOptions;
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
			_serializerOptions = new JsonSerializerOptions()
			{
				ReferenceHandler = ReferenceHandler.Preserve,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
		}
		public async Task<ResponseData<bool>> UpdateAvatar(string userUri, string imageUri, string email)
        {
			var body = new
			{
				attributes = new
				{
					avatar = new[] { imageUri }
				}, 
				email = email
			};

			var jsonBody = JsonSerializer.Serialize(body);
			var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
			
			await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
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
