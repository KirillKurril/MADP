using Microsoft.AspNetCore.Mvc;
using ALWD.API.Services.FileService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.DTOs;
using ALWD.API.Services.AccountService;

namespace ALWD.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : ControllerBase
	{
		private readonly IFileService _fileService;
        private readonly IAccountService _accountService;

		public ImageController(IFileService fileService, IAccountService accountService)
		{
			_fileService = fileService;
            _accountService = accountService;
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

			int id = fileIdResponse.Data;

			IActionResult response = await GetImage(id);

			return response;
		}
		
		[HttpGet("{id:int}")]	
		public async Task<IActionResult> GetImage(int id)
		{

            ResponseData<byte[]> fileResponse;
            try
            {
                fileResponse = await _fileService.GetFileAsByteStreamAsync(id);
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
                fileModelInfo = await _fileService.GetFileAsync(id);
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

		[HttpPost]
        public async Task<IActionResult> UploadImage(UploadImageDTO dto)
		{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IFormFile image = new Domain.DTOs.FormFile(
                new MemoryStream(dto.ImageContent!),
                "productImage",
                dto.ImageName!,
                dto.ImageMimeType!);

            ResponseData<FileModel> response;
            try
            {
                response = await _fileService.CreateFileAsync(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            if (!response.Successfull)
                return StatusCode(500, response.ErrorMessage);

            if (!string.IsNullOrEmpty(dto.UserUri))
            {
                await _accountService.
                //отдельным методом в FileService

                //int imageId = response.Data.Id;
                //Изменить avatarId;                
            }
            return Ok();
        }

    }
}