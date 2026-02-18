using Application.DTOs;

namespace Application.Features.Files.Queries.GetAll
{
    public record GetAllFilesResponse(IEnumerable<FileDTO> Files);
}
