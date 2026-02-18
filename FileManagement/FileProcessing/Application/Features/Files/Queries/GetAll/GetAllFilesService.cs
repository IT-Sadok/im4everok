using Application.Interfaces.Repositories;

namespace Application.Features.Files.Queries.GetAll
{
    public class GetAllFilesService(IFileRepository fileRepository)
    {
        public async Task<GetAllFilesResponse> Execute(GetAllFilesRequest request, CancellationToken cancellationToken)
        {
            var files = await fileRepository.GetAllAsync(cancellationToken: cancellationToken);
            return new GetAllFilesResponse(files);
        }
    }
}
