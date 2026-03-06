using Microsoft.Extensions.Logging;

namespace ProcessFileFunctionsIsolated;

public class ExtractTextFromFile
{
    private readonly ILogger<ExtractTextFromFile> _logger;

    public ExtractTextFromFile(ILogger<ExtractTextFromFile> logger)
    {
        _logger = logger;
    }

    //[Function(nameof(ExtractTextFromFile))]
    //public async Task Run(
    //    [ServiceBusTrigger("file-added-events", "text-extraction", Connection = "ServiceBusConnection")]
    //    ServiceBusReceivedMessage message,
    //    ServiceBusMessageActions messageActions)
    //{
    //    _logger.LogInformation("Message ID: {id}", message.MessageId);
    //    _logger.LogInformation("Message Body: {body}", message.Body);
    //    _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

    //        // Complete the message
    //    await messageActions.CompleteMessageAsync(message);
    //}
    //[Function(nameof(ExtractTextFromFile))]
    //public async Task<string> Run([ActivityTrigger] ActivityFunctionParameters parameters, FunctionContext context)
    //{
    //    var logger = context.GetLogger<ExtractTextFromFile>();
    //    try
    //    {
    //        logger.LogInformation($"Started text extraction for FileId: {parameters.FileId}");
    //        var ct = context.CancellationToken;

    //        string textFromDocument = await _extractTextService.ExtractTextAsync(parameters.FileSasURL, ct);
    //        // TODO: elastic search set values

    //        return textFromDocument;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogError(ex, $"Text extraction failed for FileId: {parameters.FileId}, FileName: {parameters.FileName}");
    //        throw;
    //    }
    //}
}