using Application.Common.DTOs;
using Application.Common.Interfaces.Repositories;

namespace Application.Features.Files.Queries.GetAll
{
    public class GetAllFilesService(IFileRepository fileRepository)
    {
        public async Task<IEnumerable<FileDTO>> Execute(GetAllFilesRequest request, CancellationToken cancellationToken)
        {
            return await fileRepository.GetAllAsync(cancellationToken);
        }
    }
}
