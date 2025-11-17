
using Application.Common.DTOs;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<IEnumerable<FileDTO>> GetAllAsync(CancellationToken cancellationToken);
    }
}
