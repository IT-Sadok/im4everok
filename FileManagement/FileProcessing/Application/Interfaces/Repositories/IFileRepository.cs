using Application.DTOs;

using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task Add(FileEntity file);
        Task<IEnumerable<FileDTO>> GetAllAsync(CancellationToken cancellationToken);
    }
}
