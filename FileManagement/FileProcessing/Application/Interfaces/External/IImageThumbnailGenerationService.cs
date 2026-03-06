namespace Application.Interfaces.External
{
    public interface IImageThumbnailGenerationService
    {
        Task<Stream> GenerateThumbnailAsync(Stream imageStream, int width, int height);
    }
}
