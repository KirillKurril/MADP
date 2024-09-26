

using System.ComponentModel.DataAnnotations;

namespace ALWD.Domain.DTOs
{
    public class CreateProductDTO
    {
		[Required(ErrorMessage = "Product name field is required")]
		[StringLength(100, ErrorMessage = "Max product name field length is 100 symbols")]
		public string ProductName { get; set; }


		[Required(ErrorMessage = "Product description field is required")]
		public string ProductDescription { get; set; }


		[Required(ErrorMessage = "Product price field is required")]
		[Range(0.01, 100000, ErrorMessage = "Product price value must be from 0.01 to 100000.")]
		public double ProductPrice { get; set; }


		[Required(ErrorMessage = "Product quantity field is required")]
		[Range(0.01, 100000, ErrorMessage = "Product price value must be from 1 to 100000.")]
		public int ProductQuantity { get; set; }
        
		public int ProductCategoryId { get; set; }

		
		[MaxFileSize(15 * 1024 * 1024, ErrorMessage = "Размер файла не должен превышать 5MB.")]
		public byte[]? ImageContent { get; set; }


		[AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" }, ErrorMessage = "Files with extension .jpg, .jpeg or .png are allowed only.")]
		public string? ImageName { get; set; }

		
		[AllowedMimeType(new string[] { "image/jpeg", "image/png" }, ErrorMessage = "Files with media type  image/jpeg or image/png are allowed only")]
		public string? ImageMimeType { get; set; }

	}
}
