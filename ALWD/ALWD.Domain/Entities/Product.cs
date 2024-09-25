
namespace ALWD.Domain.Entities
{
	public class Product : DbEntity
	{
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public string? ImagePath { get; set; }
		public string? ImageMimeType {  get; set; }

		public int CategoryId { get; set; }
		public Category? Category { get; set; }

		public int? FileModelId {  get; set; }
		public FileModel? Image {  get; set; }

	}
}
