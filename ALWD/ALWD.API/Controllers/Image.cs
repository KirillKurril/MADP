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
        public IActionResult PostImage(string aboba)
        {
            string _aboba = aboba;
            return Ok(_aboba);
        }
        public async Task<IActionResult> CreateImage(UploadImageDTO dto)
		{
            if (!ModelState.IsValid)
            {
				return StatusCode(500);
			}

            IFormFile image = new Domain.DTOs.FormFile(
                new MemoryStream(dto.ImageContent!),
                "productImage",
                dto.ImageName!,
                dto.ImageMimeType!);

            ResponseData<FileModel> createAvatarresponse;
            try
            {
                createAvatarresponse = await _fileService.CreateFileAsync(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            if (!createAvatarresponse.Successfull)
                return StatusCode(500, createAvatarresponse.ErrorMessage);

            if (!string.IsNullOrEmpty(dto.UserUri))
            {
                ResponseData<bool> updateAvatarResponse;
                try
                {
                    updateAvatarResponse = await _accountService.UpdateAvatar(dto.UserUri, dto.AccessToken, createAvatarresponse.Data.URL);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }

                if (!updateAvatarResponse.Successfull)
                {
                    return StatusCode(500, createAvatarresponse.ErrorMessage);
                }
            }
            return Ok();
        }

    }
}