using Amazon.S3;
using Amazon.S3.Model;

public class FilesService : IFilesService
{
    private readonly IAmazonS3 _s3Client;

    public FilesService()
    {
        var accessKey = Environment.GetEnvironmentVariable("AWS__AccessKey");
        var secretKey = Environment.GetEnvironmentVariable("AWS__SecretKey");
        var region = Environment.GetEnvironmentVariable("AWS__Region");
        var serviceURL = Environment.GetEnvironmentVariable("AWS__ServiceURL");

        var config = new AmazonS3Config();

        if (!string.IsNullOrEmpty(serviceURL))
        {
            config.ServiceURL = serviceURL;
            config.ForcePathStyle = true;
        }
        else
        {
            config.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region);
        }

        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task<string> UploadFileAsync(string bucketName, string key, Stream fileStream, string fileExtension)
    {
        if (!await DoesBucketExistAsync(bucketName))
        {
            await _s3Client.PutBucketAsync(bucketName);
        }

        string contentType = fileExtension.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            ".html" => "text/html",
            ".htm" => "text/html",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".zip" => "application/zip",
            ".rar" => "application/x-rar-compressed",
            ".tar" => "application/x-tar",
            ".gz" => "application/gzip",
            ".7z" => "application/x-7z-compressed",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".mp4" => "video/mp4",
            ".mov" => "video/quicktime",
            ".avi" => "video/x-msvideo",
            ".mkv" => "video/x-matroska",
            _ => "application/octet-stream"
        };

        string finalKey = key.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase)
                            ? key
                            : key + fileExtension;

        if (await DoesFileExistAsync(bucketName, finalKey))
        {
            throw new InvalidOperationException($"O arquivo '{finalKey}' já existe no bucket '{bucketName}'.");
        }

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = finalKey,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(request);

        return finalKey;
    }

    public async Task<Stream> GetFileAsync(string bucketName, string key)
    {
        if (!await DoesBucketExistAsync(bucketName))
        {
            throw new InvalidOperationException($"Não existe um bucket com o nome '{bucketName}'");
        }

        if (!await DoesFileExistAsync(bucketName, key))
        {
            throw new InvalidOperationException($"O arquivo não existe");
        }

        var response = await _s3Client.GetObjectAsync(bucketName, key);
        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task DeleteFileAsync(string bucketName, string key)
    {
        if (!await DoesBucketExistAsync(bucketName))
        {
            throw new InvalidOperationException($"Não existe um bucket com o nome '{bucketName}'");
        }

        if (!await DoesFileExistAsync(bucketName, key))
        {
            throw new InvalidOperationException($"O arquivo não existe");
        }

        var request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(request);
    }

    private async Task<bool> DoesBucketExistAsync(string bucketName)
    {
        var response = await _s3Client.ListBucketsAsync();
        return response.Buckets != null && response.Buckets.Any(b => b.BucketName == bucketName);
    }

    private async Task<bool> DoesFileExistAsync(string bucketName, string key)
    {
        try
        {
            var response = await _s3Client.GetObjectMetadataAsync(bucketName, key);
            return response != null;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}
