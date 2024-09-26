using Microsoft.AspNetCore.Http;

namespace ALWD.Domain.DTOs
{
	public class FormFile : IFormFile
	{
		private readonly Stream _stream;

		public FormFile(Stream stream, string name, string fileName, string contentType)
		{
			_stream = stream ?? throw new ArgumentNullException(nameof(stream));
			Name = name ?? throw new ArgumentNullException(nameof(name));
			FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
			ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
		}

		public string ContentType { get; }

		public string ContentDisposition => $"form-data; name=\"{Name}\"; filename=\"{FileName}\"";

		public IHeaderDictionary Headers => new HeaderDictionary
		{
			{ "Content-Type", ContentType },
			{ "Content-Disposition", ContentDisposition }
		};

		public long Length => _stream.Length;

		public string Name { get; }

		public string FileName { get; }

		public void CopyTo(Stream target)
		{
			_stream.Seek(0, SeekOrigin.Begin);
			_stream.CopyTo(target);
		}

		public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
		{
			_stream.Seek(0, SeekOrigin.Begin);
			await _stream.CopyToAsync(target, cancellationToken);
		}

		public Stream OpenReadStream()
		{
			_stream.Seek(0, SeekOrigin.Begin);
			return _stream;
		}
	}
}
