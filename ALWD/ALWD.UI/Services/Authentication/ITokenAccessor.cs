namespace ALWD.UI.Services.Authentication
{
	public interface ITokenAccessor
	{
		Task<string> GetAccessTokenAsync();
		Task SetAuthorizationHeaderAsync(HttpClient httpClient);
	}
}
