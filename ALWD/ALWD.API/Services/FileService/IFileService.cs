using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.FileService
{
	public interface IFileService
	{
		Task<ResponseData<FileModel>> GetFileAsync(int id);
		Task<ResponseData<int>> GetIdByNameAsync(string fileName);
		Task<ResponseData<byte[]>> GetFileAsByteStreamAsync(int id);
		Task<ResponseData<FileModel>> CreateFileAsync(IFormFile file);
		Task<ResponseData<FileModel>> UpdateFileAsync(IFormFile file);
		Task<ResponseData<FileModel>> DownloadFileAsync(int id);
		Task<ResponseData<bool>> DeleteFileAsync(int id);
	}
}
