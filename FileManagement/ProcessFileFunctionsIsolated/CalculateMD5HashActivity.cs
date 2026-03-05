using System.Text.Json;

using Application.DTOs;
using Application.Features.Files.Commands.UpdateHashNonTracking;
using Application.Interfaces.External;
using Application.Interfaces.FileStorage;

using Azure.Messaging.ServiceBus;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProcessFileFunctionsIsolated;

public class CalculateMD5Hash
{
    private readonly ILogger<CalculateMD5Hash> _logger;
    private readonly IFileStorage _fileStorage;
    private readonly IFileHashCalculationService _fileHashCalculationService;
    private readonly UpdateHashNonTrackingCommand _updateHashService;

    public CalculateMD5Hash(ILogger<CalculateMD5Hash> logger,
        IFileStorage fileStorage,
        IFileHashCalculationService fileHashCalculationService,
        UpdateHashNonTrackingCommand updateHashService)
    {
        _logger = logger;
        _fileStorage = fileStorage;
        _fileHashCalculationService = fileHashCalculationService;
        _updateHashService = updateHashService;
    }

    [Function(nameof(CalculateMD5Hash))]
    public async Task Run(
        [ServiceBusTrigger("file-uploaded-events", "md5hash", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken)
    {
        //var logger = functionContext.GetLogger<CalculateMD5HashActivity>();
        if (BinaryData.Empty == message.Body)
        {
            _logger.LogError("Message should have body to be able to process event in subscription: {0}, ", ["md5hash"]);
            throw new Exception($"Message should have body to be able to process event in subscription: md5hash");
        }

        //logger.LogInformation($"Started processing the file for MD5 hash for FileId: {parameters.FileId}, FileName: {parameters.FileName}");
        FileAddedEvent fileAddedEvent = JsonSerializer.Deserialize<FileAddedEvent>(message.Body);

        string sasUrl = await _fileStorage.GetSasUrl(fileAddedEvent.BlobPath, fileAddedEvent.ContainerName, cancellationToken);

        using var httpClient = new HttpClient();
        Uri sasURL = new(sasUrl);

        var fileBlob = await httpClient.GetStreamAsync(sasURL);

        string hash = _fileHashCalculationService.CalculateHash(fileBlob);

        UpdateHashNonTrackingRequest reqToService = new(fileAddedEvent.FileId, hash);
        await _updateHashService.Execute(reqToService, cancellationToken);

    }
}