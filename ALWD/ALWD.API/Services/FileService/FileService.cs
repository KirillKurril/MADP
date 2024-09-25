using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.FileService
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _env;
		private readonly IRepository<FileModel> _repository;

		public FileService(IWebHostEnvironment env, IRepository<FileModel> repository)
		{
			_env = env;
			_repository = repository;
		}

		public async Task<bool> DeleteFileAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseData<FileModel>> DownloadFileAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> FileExistsAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseData<FileModel>> GetFileAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseData<(byte[], string)>> GetFileAsByteStreamAsync(int id)
		{
			FileModel fileModel;

			try
			{
				fileModel = await _repository.GetByIdAsync(id);
			}
			catch(Exception ex)
			{
				return new ResponseData<(byte[], string)>((null, null), false, ex.Message);
			}
			
			byte[] file;
			
			try
			{
				file = File.ReadAllBytes(fileModel.Path);
			}
			catch (Exception ex)
			{
				return new ResponseData<(byte[], string)>((null, null), false, "Failure of reading file in fileservice");
			}

			return new ResponseData<(byte[], string)>((file, fileModel.MimeType), true);
		}

		public async Task<ResponseData<FileModel>> UpdateFileAsync(int id, IFormFile file)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseData<FileModel>> UploadFileAsync(IFormFile file)
		{
			throw new NotImplementedException();
		}
	}
}
