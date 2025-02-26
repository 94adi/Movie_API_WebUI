using Azure.Storage.Files.Shares;
using Azure.Storage.Sas;

namespace Movie.API.Services.File;

public class AzureFileShareService : IFileShareService
{
    private readonly FileShareConfig _fileShareConfig;
    private readonly ShareClient _shareClient;

    public AzureFileShareService(IOptions<FileShareConfig> config)
    {
        _fileShareConfig = config.Value;
        _shareClient = new ShareClient(_fileShareConfig.ConnectionString, 
            _fileShareConfig.ShareName);
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var rootDirectory = _shareClient.GetRootDirectoryClient();
        var fileClient = rootDirectory.GetFileClient(fileName);

        if (await fileClient.ExistsAsync())
        {
            var downloadInfo = await fileClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }

        throw new FileNotFoundException("File not found in file share.");
    }

    public async Task UploadFileAsync(string fileName, Stream fileStream)
    {
        await _shareClient.CreateIfNotExistsAsync();

        var rootDirectory = _shareClient.GetRootDirectoryClient();

        var fileClient = rootDirectory.GetFileClient(fileName);

        await fileClient.CreateAsync(fileStream.Length);

        await fileClient.UploadAsync(fileStream);
    }

    public string GenerateFileUrl(string fileName, TimeSpan expirationTime)
    {
        string appUrl = Utilities.GetAppUrl();
        string defaultImage = $"{appUrl}/LocalPosters/default.jpg";

        if (string.IsNullOrEmpty(fileName))
        {
            return defaultImage;
        }

        try
        {          
            var rootDirectory = _shareClient.GetRootDirectoryClient();
            var fileClient = rootDirectory.GetFileClient(fileName);

            var sasUri = fileClient.GenerateSasUri(
                ShareFileSasPermissions.Read,
                DateTimeOffset.UtcNow.Add(expirationTime)
                ).ToString();

            if (string.IsNullOrEmpty(sasUri))
            {
                return defaultImage;
            }

            return sasUri;
        }
        catch(Exception e)
        {
        }

        return defaultImage;
    }
}
