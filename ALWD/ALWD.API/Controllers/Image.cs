using Microsoft.AspNetCore.Mvc;

namespace ALWD.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : ControllerBase
	{
		private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

		[HttpGet("{imageName}")]
		public IActionResult GetImage(string imageName)
		{
			var filePath = Path.Combine(_imagesPath, imageName);

			if (!System.IO.File.Exists(filePath))
			{
				return NotFound();
			}

			var mimeType = GetMimeType(filePath);
			var fileBytes = System.IO.File.ReadAllBytes(filePath);

			return File(fileBytes, mimeType);
		}

		private string GetMimeType(string filePath)
		{
			var extension = Path.GetExtension(filePath).ToLowerInvariant();
			return extension switch
			{
				".jpg" => "image/jpeg",
				".jpeg" => "image/jpeg",
				".png" => "image/png",
				".gif" => "image/gif",
				_ => "application/octet-stream",
			};
		}
	}
}