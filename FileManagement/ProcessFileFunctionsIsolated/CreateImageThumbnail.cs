using System.Text.Json;

using Application.DTOs;
using Application.Interfaces.External;
using Application.Interfaces.FileStorage;

using Azure.Messaging.ServiceBus;

using Infrastructure.Options;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProcessFileFunctionsIsolated;

public class CreateImageThumbnail
{
    private readonly ILogger<CreateImageThumbnail> _logger;
    private readonly IImageThumbnailGenerationService _imageThumbnailGenerationService;
    private readonly IFileStorage _fileStorage;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ThumbnailOptions _thumbnailsOptions;

    public CreateImageThumbnail(ILogger<CreateImageThumbnail> logger,
        IImageThumbnailGenerationService imageThumbnailGenerationService,
        IFileStorage fileStorage,
        IHttpClientFactory httpClientFactory,
        IOptions<ThumbnailOptions> thumbnailsOptionsObj)
    {
        _logger = logger;
        _imageThumbnailGenerationService = imageThumbnailGenerationService;
        _fileStorage = fileStorage;
        _httpClientFactory = httpClientFactory;
        _thumbnailsOptions = thumbnailsOptionsObj.Value;
    }

    [Function("CreateImageThumbnail")]
    public async Task<string> Run([ServiceBusTrigger("file-uploaded-events", "thumbnail", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken)
    {
        //var logger = functionContext.GetLogger<CreateImageThumbnail>();
        try
        {
            if (BinaryData.Empty == message.Body)
            {
                _logger.LogError("Message should have body to be able to process event in subscription: {0}, ", ["thumbnail"]);
                throw new Exception($"Message should have body to be able to process event in subscription: thumbnail");
            }

            //logger.LogInformation($"Started creating Image thumbnail for FileId: {parameters.FileId}, FileName: {parameters.FileName}");
            FileAddedEvent fileAddedEvent = JsonSerializer.Deserialize<FileAddedEvent>(message.Body);

            string sasUrl = await _fileStorage.GetSasUrl(fileAddedEvent.BlobPath, fileAddedEvent.ContainerName, cancellationToken);

            var httpClient = _httpClientFactory.CreateClient("AzureBlobDownloader");
            using var file = await httpClient.GetStreamAsync(sasUrl);

            using var thumbnail = await _imageThumbnailGenerationService.GenerateThumbnailAsync(file,
                _thumbnailsOptions.Width, _thumbnailsOptions.Height);

            await _fileStorage.UploadAsync($"thumbnails/{fileAddedEvent.FileId.ToString()}.webp",
                thumbnail,
                _thumbnailsOptions.ContainerName,
                "image/webp",
                cancellationToken);

            return fileAddedEvent.FileId.ToString();
        }
        catch (Exception ex)
        {
            //logger.LogError(ex, $"Failed creating Image thumbnail for FileId: {parameters.FileId}, FileName: {parameters.FileName}");
            throw;
        }
    }
}