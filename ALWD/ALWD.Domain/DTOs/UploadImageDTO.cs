using System.ComponentModel.DataAnnotations;

namespace ALWD.Domain.DTOs
{
    public class UploadImageDTO
    {
        [Required(ErrorMessage = "Image content is required")]
        [MaxFileSize(15 * 1024 * 1024, ErrorMessage = "Размер файла не должен превышать 15MB.")]
        public byte[] ImageContent { get; set; } = null!;

        [Required(ErrorMessage = "Image name is required")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" }, ErrorMessage = "Files with extension .jpg, .jpeg or .png are allowed only.")]
        public string ImageName { get; set; } = null!;

        [Required(ErrorMessage = "Image media type property is required")]
        [AllowedMimeType(new string[] { "image/jpeg", "image/png" }, ErrorMessage = "Files with media type  image/jpeg or image/png are allowed only")]
        public string? ImageMimeType { get; set; } = null!;

        public string Email {  get; set; } = string.Empty;

        public string UserUri { get; set; } = string.Empty;
	}
}
