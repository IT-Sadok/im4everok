namespace Application.Interfaces.FileStorage
{
    public interface IFileStorage
    {
        Task UploadAsync(string path, Stream content, string containerName, string? contentType, CancellationToken cancellationToken);
    }
}
