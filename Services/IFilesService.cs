public interface IFilesService
{
    Task<string> UploadFileAsync(string bucketName, string key, Stream fileStream, string fileExtension);
    Task<Stream> GetFileAsync(string bucketName, string key);
    Task DeleteFileAsync(string bucketName, string key);
}
