namespace ALWD.UI.Models
{
    public class UriData
    {
        public static string ApiUri { get; set; } = string.Empty;

        public UriData(IConfiguration config)
        {
            try
            {
                ApiUri = config.GetSection("UriData").GetValue<string>("ApiUri");
            }
            catch
            {
                throw new Exception("Unable to receive UriData/ApiUri configuration");
            }
        }
    }
}