
using System;

namespace Movie.API.Services.File;

public class LocalFileShareService : IFileShareService
{
    private readonly FileLocalConfig _fileLocalConfig;

    public LocalFileShareService(IOptions<FileLocalConfig> fileLocalConfigOptions)
    {
        _fileLocalConfig = fileLocalConfigOptions.Value;
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var filePath = GenerateFilePath(fileName);

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return fileStream;
    }

    public string GenerateFileUrl(string fileName, TimeSpan expirationTime)
    {
        string appUrl = Utilities.GetAppUrl();
        string defaultImage = $"{appUrl}/LocalPosters/default.jpg";

        if (string.IsNullOrEmpty(fileName))
        {
            return defaultImage;
        }

        var baseUrl = Utilities.GetAppUrl();
        string imageUrl = baseUrl + _fileLocalConfig.ImageStorageFolder + fileName;

        var result = imageUrl ??= "https://placehold.co/600x400";

        return result;
    }

    public async Task UploadFileAsync(string fileName, Stream fileStream)
    {
        var filePath = GenerateFilePath(fileName);

        var folderLocation = Path.Combine(Directory.GetCurrentDirectory(),
            _fileLocalConfig.ImageStoragePath);

        if (!Directory.Exists(folderLocation))
        {
            Directory.CreateDirectory(folderLocation);
        }

        using var fileStreamToWrite = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fileStreamToWrite);
    }

    private string GenerateFilePath(string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            _fileLocalConfig.ImageStoragePath,
                            fileName);

        return filePath;
    }
}
