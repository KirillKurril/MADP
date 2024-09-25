using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.FileService
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _env;
		private readonly IRepository<FileModel> _repository;
		private readonly string _filesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

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
			FileModel fileModel;
			try
			{
				fileModel = await _repository.GetByIdAsync(id);
			}
			catch (Exception ex)
			{
				return new ResponseData<FileModel>(null, false, ex.Message);
			}
			return new ResponseData<FileModel>(fileModel);
		}

		public async Task<ResponseData<int>> GetIdByNameAsync(string fileName)
		{
			IReadOnlyList<FileModel> fileCollection;

			try
			{
				fileCollection = await _repository.ListAsync(f => f.Name == fileName);
			}
			catch (Exception ex)
			{
				return new ResponseData<int>(0, false, ex.Message);
			}

			if (fileCollection.Count == 0)
				return new ResponseData<int>(0, false, $"file with requested name {fileName} doesn't found");

			return new ResponseData<int>(fileCollection.ElementAt(0).Id);
		}
		public async Task<ResponseData<byte[]>> GetFileAsByteStreamAsync(int id)
		{
			FileModel fileModel;

			try
			{
				fileModel = await _repository.GetByIdAsync(id);
			}
			catch(Exception ex)
			{
				return new ResponseData<byte[]>(null, false, ex.Message);
			}
			
			byte[] file;
			string filePath = string.Join("\\", _filesPath, "images", fileModel.Name);
			try
			{
				file = File.ReadAllBytes(filePath);
			}
			catch (Exception ex)
			{
				return new ResponseData<byte[]>(null, false, "Failure of reading file. class : FileSerivce, method: GetFileAsByteStreamAsync");
			}

			return new ResponseData<byte[]>(file, true);
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
