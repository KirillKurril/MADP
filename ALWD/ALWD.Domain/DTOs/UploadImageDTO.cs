using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWD.Domain.DTOs
{
    public class UploadImageDTO
    {
        [Required(ErrorMessage = "Image content is required")]
        [MaxFileSize(15 * 1024 * 1024, ErrorMessage = "Размер файла не должен превышать 5MB.")]
        public byte[]? ImageContent { get; set; }

        [Required(ErrorMessage = "Image name is required")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" }, ErrorMessage = "Files with extension .jpg, .jpeg or .png are allowed only.")]
        public string? ImageName { get; set; }

        [Required(ErrorMessage = "Image media type property is required")]
        [AllowedMimeType(new string[] { "image/jpeg", "image/png" }, ErrorMessage = "Files with media type  image/jpeg or image/png are allowed only")]
        public string? ImageMimeType { get; set; }

        public string UserUri { get; set; } = string.Empty;
		public string AccessToken { get; set; }

	}
}
