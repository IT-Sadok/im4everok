using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;

using Domain.Entities;

namespace Application.Features.Files.Commands.Upload
{
    public class UploadFileService(IFileRepository fileRepository, IUnitOfWork unitOfWork)
    {
        public async Task<UploadFileResponse> Execute(UploadFileRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var file = new FileEntity
                {
                    CreatedAt = DateTime.Now,
                    FileName = request.File.FileName,
                    Size = request.File.Size,
                };

                await fileRepository.Add(file);
                await unitOfWork.SaveChanges();

                return new UploadFileResponse();
            }
            catch
            {
                return new UploadFileResponse();
            }
        }
    }
}
