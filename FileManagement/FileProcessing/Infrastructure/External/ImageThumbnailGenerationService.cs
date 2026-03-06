
using Application.Interfaces.External;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.External
{
    internal class ImageThumbnailGenerationService : IImageThumbnailGenerationService
    {
        public async Task<Stream> GenerateThumbnailAsync(Stream imageStream, int width, int height)
        {
            using var image = await Image.LoadAsync(imageStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            }));

            var output = new MemoryStream();
            await image.SaveAsWebpAsync(output);

            output.Position = 0;
            return output;
        }
    }
}
