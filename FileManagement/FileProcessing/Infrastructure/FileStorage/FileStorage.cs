
using Application.Interfaces.FileStorage;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.FileStorage
{
    internal class AzureFileStorage(IOptions<FileStorageConfiguration> optionsDI, ILogger<AzureFileStorage> logger) : IFileStorage
    {
        private FileStorageConfiguration config = optionsDI.Value;
        public async Task DeleteAsync(string path, string containerName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(config.ConnectionName)) throw new Exception("Blob storage connection string is not defined");

            BlobContainerClient containerClient = new(config.ConnectionName, containerName);
            await containerClient.DeleteBlobIfExistsAsync(path, cancellationToken: cancellationToken);
        }

        private void ThrowExceptionIfNoConnectionStringToBlob()
        {
            if (string.IsNullOrEmpty(config.ConnectionName)) throw new Exception("Blob storage connection string is not defined");
        }

        public async Task UploadAsync(string path, Stream content,
            string containerName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfNoConnectionStringToBlob();

            BlobContainerClient containerClient = new(config.ConnectionName, containerName);

            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blob = containerClient.GetBlobClient(path);

            var headers = new BlobHttpHeaders();
            if (!string.IsNullOrEmpty(contentType)) headers.ContentType = contentType;

            var infoResponse = await blob.UploadAsync(content, new BlobUploadOptions
            {
                HttpHeaders = headers,
            }, cancellationToken: cancellationToken);
        }

        public async Task<string> GetSasUrl(string blobPath, string containerName, CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfNoConnectionStringToBlob();

            BlobContainerClient containerClient = new(config.ConnectionName, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobPath);

            BlobSasBuilder blobSasBuilder = new(BlobSasPermissions.All, DateTimeOffset.Now.AddMinutes(10));

            Uri sasUrl = blobClient.GenerateSasUri(blobSasBuilder);
            return sasUrl.AbsoluteUri;
        }

        public async Task<Stream?> GetFileStream(string blobPath, string containerName, CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfNoConnectionStringToBlob();

            BlobContainerClient containerClient = new(config.ConnectionName, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobPath);

            var fileStream = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);

            try
            {
                return fileStream.Value.Content;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetFileStream failed, blobPath: {blobPath}, containerName: {containerName}");
                return null;
            }
        }
    }
}
