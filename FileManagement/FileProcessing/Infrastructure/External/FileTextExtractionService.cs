
using Application.Interfaces.External;

using Azure;
using Azure.AI.DocumentIntelligence;

using Infrastructure.Options;

using Microsoft.Extensions.Options;

namespace Infrastructure.External
{
    internal class FileTextExtractionService : IFileTextExtractionService
    {
        private readonly DocumentIntelligenceClient _client;

        public FileTextExtractionService(IOptions<FileTextExtractionOptions> options)
        {
            var credential = new AzureKeyCredential(options.Value.ApiKey);
            _client = new DocumentIntelligenceClient(new Uri(options.Value.Endpoint), credential);
        }

        public async Task<string> ExtractTextAsync(string sasURL, CancellationToken cancellationToken)
        {
            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-read",
                new Uri(sasURL),
                cancellationToken);

            var result = await operation.WaitForCompletionAsync(cancellationToken);

            return string.Join(
                Environment.NewLine,
                result.Value.Content);
        }
    }
}
