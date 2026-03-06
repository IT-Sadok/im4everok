using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.FileStorage;
using Application.Interfaces.Repositories;

using Domain.Entities;

namespace Application.Features.Files.Commands.Upload
{
    public class UploadFileCommand(IFileRepository fileRepository,
        IOutboxEventRepository outboxRepository,
        IUnitOfWork unitOfWork,
        IFileStorage fileStorage)
    {
        public async Task<UploadFileResponse> Execute(UploadFileRequest request, CancellationToken ct)
        {
            try
            {
                Guid fileId = Guid.NewGuid();

                var file = new FileEntity
                {
                    Id = fileId,
                    CreatedAt = DateTime.UtcNow,
                    FileName = request.FileName,
                    SizeBytes = request.Content.Length,
                    ContentType = request.ContentType,
                    ContainerName = "files",
                    BlobPath = fileId.ToString(),
                };

                var fileAddedEvent = new FileAddedEvent()
                {
                    FileId = file.Id,
                    FileName = file.FileName,
                    SizeBytes = file.SizeBytes,
                    ContentType = file.ContentType,
                    BlobPath = file.BlobPath,
                    ContainerName = file.ContainerName,
                    CreatedAtUtc = file.CreatedAt
                };

                var outboxEvent = new OutboxEvent()
                {
                    Content = System.Text.Json.JsonSerializer.Serialize(fileAddedEvent),
                    Id = Guid.NewGuid(),
                    Error = null,
                    OccuredOnUtc = DateTime.UtcNow,
                    RetryCount = 0,
                    Type = "FileAddedEvent"
                };

                await fileRepository.Add(file);
                await outboxRepository.Add(outboxEvent);
                await unitOfWork.SaveChangesAsync();

                await fileStorage.UploadAsync(fileId.ToString(), request.Content, "files", request.ContentType, ct);

                return new UploadFileResponse(fileId.ToString());
            }
            catch
            {
                return new UploadFileResponse(null);
            }
        }
    }
}
