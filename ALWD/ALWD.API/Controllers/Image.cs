using Microsoft.AspNetCore.Mvc;
using ALWD.API.Services.FileService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : ControllerBase
	{
		private readonly IFileService _fileService;

		public ImageController(IFileService fileService)
		{
			_fileService = fileService;
		}

		[HttpGet("{imageName}")]
		public async Task<IActionResult> GetImage(string imageName)
		{
			ResponseData<int> fileIdResponse;
			try
			{
				fileIdResponse = await _fileService.GetIdByNameAsync(imageName);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			if (!fileIdResponse.Successfull)
				return BadRequest(fileIdResponse.ErrorMessage);


			ResponseData<byte[]> fileResponse;
			try
			{
				fileResponse = await _fileService.GetFileAsByteStreamAsync(fileIdResponse.Data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			if (!fileResponse.Successfull)
				return BadRequest(fileResponse.ErrorMessage);

			if (fileResponse.Data == null)
				return NotFound();

			ResponseData<FileModel> fileModelInfo;

			try
			{
				fileModelInfo = await _fileService.GetFileAsync(fileIdResponse.Data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			if (!fileModelInfo.Successfull)
				return BadRequest(fileModelInfo.ErrorMessage);

			if (fileModelInfo.Data == null)
				return NotFound();

			return File(fileResponse.Data, fileModelInfo.Data.MimeType);
		}
	}
}