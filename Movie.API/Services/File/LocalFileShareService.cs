
namespace Movie.API.Services.File;

public class LocalFileShareService : IFileShareService
{
    private readonly FileLocalConfig _fileLocalConfig;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalFileShareService(IOptions<FileLocalConfig> fileLocalConfigOptions, 
        IHttpContextAccessor httpContextAccessor)
    {
        _fileLocalConfig = fileLocalConfigOptions.Value;
        _httpContextAccessor = httpContextAccessor;
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

        var httpContext = _httpContextAccessor.HttpContext;
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/{httpContext.Request.PathBase.Value}";
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
