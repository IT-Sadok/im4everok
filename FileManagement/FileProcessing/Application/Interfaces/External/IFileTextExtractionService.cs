namespace Application.Interfaces.External
{
    public interface IFileTextExtractionService
    {
        Task<string> ExtractTextAsync(string sasURL, CancellationToken cancellationToken);
    }
}
