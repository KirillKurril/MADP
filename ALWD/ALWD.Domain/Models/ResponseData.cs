namespace ALWD.Domain.Models
{
	public class ResponseData<T>
	{
		public ResponseData(T data) => Data = data;
		public T Data { get; set; }
		public bool Success { get; set; } = true;
		public string? ErrorMessage { get; set; }
	}

}
