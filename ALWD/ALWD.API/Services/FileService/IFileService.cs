using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.FileService
{
	public interface IFileService
	{
		Task<ResponseData<FileModel>> GetFileAsync(int id);
		Task<ResponseData<int>> GetIdByNameAsync(string fileName);
		Task<ResponseData<byte[]>> GetFileAsByteStreamAsync(int id);
		Task<ResponseData<FileModel>> UploadFileAsync(IFormFile file);
		Task<ResponseData<FileModel>> UpdateFileAsync(int id, IFormFile file);
		Task<ResponseData<FileModel>> DownloadFileAsync(int id);
		Task<bool> DeleteFileAsync(int id);
		Task<bool> FileExistsAsync(int id);
	}
}
