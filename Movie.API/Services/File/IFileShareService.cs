namespace Movie.API.Services.File;

public interface IFileShareService
{
    Task UploadFileAsync(string fileName, Stream fileStream);
    Task<Stream> DownloadFileAsync(string fileName);
    string GenerateFileUrl(string fileName, TimeSpan expirationTime);
}
