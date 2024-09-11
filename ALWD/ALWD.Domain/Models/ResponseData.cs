using Newtonsoft.Json;

namespace ALWD.Domain.Models
{
	public class ResponseData<T>
	{
        public ResponseData( T? data, bool successfull = true, string errorMessage = "it's ok")
        {
            Data = data;
            Successfull = successfull;
            ErrorMessage = errorMessage;
        }
        public T Data { get; set; }
		public bool Successfull { get; set; } = true;
		public string? ErrorMessage { get; set; }
	}

}
