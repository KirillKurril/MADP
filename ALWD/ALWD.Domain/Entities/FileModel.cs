namespace ALWD.Domain.Entities
{
	public class FileModel : DbEntity
	{
		public string Name { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public string MimeType {  get; set; } = string.Empty;
	}
}
