using System.Linq.Expressions;

using Application.DTOs;

using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task Add(FileEntity file);
        Task<IEnumerable<FileDTO>> GetAllAsync(Expression<Func<FileEntity, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task<FileDTO?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken = default);
        Task<FileDTO?> GetByFileNameAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
