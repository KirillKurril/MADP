

namespace ALWD.Domain.DTOs
{
    public class CreateProductDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductCategoryId { get; set; }
        public byte[]? ImageContent { get; set; }
        public string? ImageName { get; set; }
        public string? ImageMimeType { get; set; }
    }
}
