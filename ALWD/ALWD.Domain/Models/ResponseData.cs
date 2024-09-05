namespace ALWD.Domain.Models
{
	public class ResponseData<T>
	{
		public ResponseData(T data, bool success = true, string errorMessage = "it's ok")
			=> (Data, Successfull, ErrorMessage) = (data, success, errorMessage);
		public T Data { get; set; }
		public bool Successfull { get; set; } = true;
		public string? ErrorMessage { get; set; }
	}

}
