namespace Application.Features.Files.Commands.Upload
{
    public record UploadFileRequest(string FileName, Stream Content, string ContentType);
}
