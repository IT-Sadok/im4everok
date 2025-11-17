using Application.Common.DTOs;

namespace Application.Features.Files.Queries.GetAll
{
    public record GetAllFilesResponse(IEnumerable<FileDTO> Files);
}
