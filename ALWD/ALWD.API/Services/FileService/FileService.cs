using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.API.Services.FileService
{
	public class FileService : IFileService
	{
		private readonly IRepository<FileModel> _repository;
		private readonly string _filesPath;
		private readonly string _filesUri;

		public FileService(IWebHostEnvironment env, IRepository<FileModel> repository, [FromServices] IConfiguration config)
		{
			_repository = repository;
			_filesPath = env.WebRootPath;
			_filesUri = config["APIUri"];
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
			try
			{
				file = File.ReadAllBytes(fileModel.Path);
			}
			catch (Exception ex)
			{
				return new ResponseData<byte[]>(null, false, "Failure of reading file. class : FileSerivce, method: GetFileAsByteStreamAsync");
			}

			return new ResponseData<byte[]>(file, true);
		}

		public async Task<ResponseData<FileModel>> CreateFileAsync(IFormFile file)
		{
			if (file == null)
				return new ResponseData<FileModel>(null, false, "class FileService, method CreateFileAsync: FormFile is null");
			
			if(file.Length == 0)
				return new ResponseData<FileModel>(null, false, "class FileService, method CreateFileAsync: FormFile is empty");

			if (string.IsNullOrEmpty(file.FileName))
				return new ResponseData<FileModel>(null, false, "class FileService, method CreateFileAsync: File name is empty");

			if(string.IsNullOrEmpty(file.ContentType))
				return new ResponseData<FileModel>(null, false, "class FileService, method CreateFileAsync: File media type is empty");


			FileModel fileModel = new FileModel()
			{
				MimeType = file.ContentType,
				Name = file.FileName
			};

			int lastDotIndex = fileModel.Name.LastIndexOf('.');
			string nameWithoutExtension = fileModel.Name.Substring(0, lastDotIndex);
			string extension = fileModel.Name.Substring(lastDotIndex);
			string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
			
			fileModel.Name = $"{nameWithoutExtension}-{timestamp}{extension}";

			string fileDirectory = fileModel.MimeType.Split('/')[0];

			string filePath = string.Join('\\', _filesPath, fileDirectory, fileModel.Name);

			fileModel.Path = filePath;

			string controllerName = char.ToUpper(fileDirectory[0]) + fileDirectory.Substring(1);
			fileModel.URL = $"{_filesUri}/{controllerName}/{fileModel.Name}";

			try
			{
				await _repository.AddAsync(fileModel);
			}
			catch (Exception ex)
			{
				return new ResponseData<FileModel>(null, false, $"class FileService, method CreateFileAsync: File writing to DB exception: {ex.Message}");
			}

			try
			{
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
			}
			catch (Exception ex)
			{
				return new ResponseData<FileModel>(null, false, "class FileService, method CreateFileAsync: File writing failure");
			}

			return new ResponseData<FileModel>(fileModel); 
		}
		
		public async Task<ResponseData<FileModel>> UpdateFileAsync(IFormFile file)
		{
			if (file == null)
				return new ResponseData<FileModel>(null, true);

			ResponseData<FileModel> createResponse;
			try
			{
				createResponse = await CreateFileAsync(file);
			}
			catch (Exception ex)
			{
				return new ResponseData<FileModel>(null, false, $"class FileService, method UpdateFileAsync: FormFile {file.FileName} creation failure: {ex.Message}");
			}

			if (!createResponse.Successfull)
				return new ResponseData<FileModel>(null, false, createResponse.ErrorMessage);

			return createResponse;
		}
		
		public async Task<ResponseData<bool>> DeleteFileAsync(int id)
		{
			bool exist = await _repository.Exists(id);
			if (!exist)
				return new ResponseData<bool>(false, false, $"class FileService, method DeleteFileAsync: FormFile with id {id} doesn't exists");

			FileModel fileModel = GetFileAsync(id).Result.Data;

			try
			{
				await _repository.DeleteAsync(fileModel);
			}
			catch (Exception ex)
			{
				return new ResponseData<bool>(false, false, $"class FileService, method DeleteFileAsync: FormFile with id {id} removing from DB failure: {ex.Message}");
			}

			try
			{
				File.Delete(fileModel.Path);
			}
			catch (Exception ex)
			{
				return new ResponseData<bool>(false, false, $"class FileService, method DeleteFileAsync: FormFile with id {id} removing from disk failure: {ex.Message}");
			}

			return new ResponseData<bool>(true);
		}
	}
}
