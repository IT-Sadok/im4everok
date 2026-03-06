using Application.DTOs;
using Application.Interfaces.FileStorage;
using Application.Interfaces.Repositories;

namespace Application.Features.Files.Commands.Delete
{
    public class DeleteFileCommand(IFileRepository fileRepository, IFileStorage fileStorage)
    {
        public async Task Execute(DeleteFileRequest request, CancellationToken ct)
        {
            FileDTO? file = null;
            if (request.FileId is not null)
            {
                file = await fileRepository.GetByIdAsync(Guid.Parse(request.FileId), ct);
            }
            else if (request.FileName is not null)
            {
                file = await fileRepository.GetByFileNameAsync(request.FileName, ct);
            }
            // TODO: move to validation
            //else
            //{
            //    throw new ArgumentException("Either FileId or FileName must be provided.");
            //}

            if (file is null)
            {
                throw new KeyNotFoundException("File not found.");
            }

            await fileStorage.DeleteAsync(file.BlobPath, file.ContainerName, ct);
        }
    }
}
