using Application.Interfaces;
using Application.Interfaces.FileStorage;
using Application.Interfaces.Repositories;

using Domain.Entities;

namespace Application.Features.Files.Commands.Upload
{
    public class UploadFileService(IFileRepository fileRepository, IUnitOfWork unitOfWork, IFileStorage fileStorage)
    {
        public async Task<UploadFileResponse> Execute(UploadFileRequest request, CancellationToken ct)
        {
            try
            {
                Guid fileId = Guid.NewGuid();

                await fileStorage.UploadAsync(fileId.ToString(), request.Content, "files", request.ContentType, ct);

                var file = new FileEntity
                {
                    Id = fileId,
                    CreatedAt = DateTime.Now,
                    FileName = request.FileName,
                    SizeBytes = request.Content.Length,
                    ContentType = request.ContentType,
                };

                await fileRepository.Add(file);
                await unitOfWork.SaveChangesAsync();

                return new UploadFileResponse(fileId.ToString());
            }
            catch
            {
                return new UploadFileResponse(null);
            }
        }
    }
}
