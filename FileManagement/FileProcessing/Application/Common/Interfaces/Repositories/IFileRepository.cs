
using Application.Common.DTOs;

using Domain.Entities;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task Add(FileEntity file);
        Task<IEnumerable<FileDTO>> GetAllAsync(CancellationToken cancellationToken);
    }
}
