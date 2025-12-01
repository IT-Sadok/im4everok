
using Application.Interfaces.FileStorage;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.FileStorage
{
    internal class FileStorage(IConfiguration config) : IFileStorage
    {
        public async Task UploadAsync(string path, Stream content,
            string containerName,
            string contentType = null,
            CancellationToken cancellationToken = default)
        {
            string? connectionString = config.GetSection("BlobConnectionString").Value;

            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Blob storage connection string is not defined");

            BlobContainerClient containerClient = new(connectionString, containerName);

            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blob = containerClient.GetBlobClient(path);

            var headers = new BlobHttpHeaders();
            if (!string.IsNullOrEmpty(contentType)) headers.ContentType = contentType;

            var infoResponse = await blob.UploadAsync(content, new BlobUploadOptions
            {
                HttpHeaders = headers,
            }, cancellationToken: cancellationToken);
        }
    }
}
