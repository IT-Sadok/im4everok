namespace Application.Interfaces.FileStorage
{
    public interface IFileStorage
    {
        Task UploadAsync(string path, Stream content, string containerName, string? contentType, CancellationToken cancellationToken);
        Task DeleteAsync(string path, string containerName, CancellationToken cancellationToken);
        Task<string> GetSasUrl(string blobPath, string containerName, CancellationToken cancellationToken);
        Task<Stream?> GetFileStream(string blobPath, string containerName, CancellationToken cancellationToken = default);
    }
}
