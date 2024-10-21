using ALWD.UI.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using ALWD.Domain.Models;
using ALWD.UI.Services.ProductService;
using ALWD.Domain.DTOs;
using ALWD.Domain.Services.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Azure;
using System.Configuration;
using System.Text.Json.Serialization;


namespace ALWD.UI.Services.Authentication
{
    public class KeycloakAuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ITokenAccessor _tokenAccessor;
		JsonSerializerOptions _serializerOptions;
		private readonly IHttpContextAccessor _httpContextAccessor;
        ILogger<ApiProductService> _logger;
        KeycloakData _keycloakData;


        public KeycloakAuthService(HttpClient httpClient,
            IOptions<KeycloakData> options,
            ITokenAccessor tokenAccessor,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _tokenAccessor = tokenAccessor;
            _keycloakData = options.Value;
            _configuration = config;
			_serializerOptions = new JsonSerializerOptions()
			{
				ReferenceHandler = ReferenceHandler.Preserve,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
		}
        public async Task<ResponseData<bool>> RegisterUserAsync(string email, string password, string? name, IFormFile? image)
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
            newUser.Attributes.Add("avatarId", "https://localhost:7002/api/Image/default-profile-picture.png");
            newUser.Email = email;
            newUser.Username = name ?? email;
            newUser.Credentials.Add(new UserCredentials { Value = password });
            
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var userData = JsonSerializer.Serialize(newUser, serializerOptions);
            HttpContent content = new StringContent(userData, Encoding.UTF8, "application/json");

            var registrationResponse = await _httpClient.PostAsync($"{_keycloakData.Host}/admin/realms/{_keycloakData.Realm}/users", content);

            if (!registrationResponse.IsSuccessStatusCode)
                return new ResponseData<bool>(false, false, registrationResponse.StatusCode + " " + registrationResponse.Content);

            string userUri = registrationResponse.Headers.Location!.AbsoluteUri;

			if (image != null)
            {
				var uri = $"{_httpClient.BaseAddress.AbsoluteUri}Image";

				await using var ms = new MemoryStream();
				await image.CopyToAsync(ms);

				UploadImageDTO dto = new()
				{
					ImageContent = ms.ToArray(),
					ImageMimeType = image.ContentType,
					ImageName = image.Name,
					UserUri = userUri,
					AccessToken = await _tokenAccessor.GetAccessTokenAsync()
				};


				HttpResponseMessage avatarUpdateResponse = await _httpClient.PostAsJsonAsync(uri, dto, _serializerOptions);

                if (!avatarUpdateResponse.IsSuccessStatusCode)
                    return new ResponseData<bool>(false, false, avatarUpdateResponse.StatusCode + " " + avatarUpdateResponse.ReasonPhrase);
            }

            var signInResponse = await SignIn(registrationResponse);
            return signInResponse;
        }
    
        public async Task<ResponseData<bool>> AuthenticateUser(string username, string password)
        {
            HttpResponseMessage authResponse;
            try
            {
                await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

                var tokenEndpoint = $"{_keycloakData.Host}/realms/{_keycloakData.Realm}/protocol/openid-connect/token";

                var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("client_id", _keycloakData.ClientId),
                        new KeyValuePair<string, string>("client_secret", _keycloakData.ClientSecret),
                        new KeyValuePair<string, string>("grant_type", "password"),
                    }); 

                authResponse = await _httpClient.PostAsync(tokenEndpoint, content);
                if (!authResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Authentication failed");
                }
            }
            catch (Exception ex)
            {
                return new ResponseData<bool>(false, false, ex.Message);
            }
                var signInResponse = await SignIn(authResponse);
                return signInResponse;

               
        }

        private async Task<ResponseData<bool>> SignIn (HttpResponseMessage authResponse)
        {
            try 
            {
                var jsonString = await authResponse.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonString);
                var accessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

                var username = jsonToken.Claims.First(claim => claim.Type == "name").Value;
                var userId = jsonToken.Claims.First(claim => claim.Type == "sub").Value;
                var email = jsonToken.Claims.First(claim => claim.Type == "email").Value;
                var roles = jsonToken.Claims.Where(claim => claim.Type == "role").Select(claim => claim.Value).ToList();
                var avatarUri = jsonToken.Claims.First(claim => claim.Type == "avatar").Value;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email),
                    new Claim("userId", userId),
                    new Claim("avatar", avatarUri),
                    new Claim("accessToken", accessToken)
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var context = _httpContextAccessor.HttpContext;
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            }
            catch (Exception ex)
            {
                return new ResponseData<bool>(false, false, ex.Message);
            }
            
            return new ResponseData<bool>(true);
        }

        public async Task<ResponseData<bool>> LogOut()
        {
            try
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var logoutEndpoint = $"{_keycloakData.Host}/realms/{_keycloakData.Realm}/protocol/openid-connect/logout";

                var request = new HttpRequestMessage(HttpMethod.Post, logoutEndpoint);
                request.Content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", _keycloakData.ClientId),
                new KeyValuePair<string, string>("client_secret", _keycloakData.ClientSecret)
                });

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ResponseData<bool>(true);
                }

                return new ResponseData<bool>(false, false, response.StatusCode + " " + response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return new ResponseData<bool>(false, false, ex.Message);
            }
        }
    }
}
