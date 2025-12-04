using System.Linq.Expressions;

using Application.DTOs;
using Application.Interfaces.Repositories;

using Domain.Entities;

using Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repositories
{
    internal class FileRepository(AppDbContext context) : IFileRepository
    {
        public Task Add(FileEntity file)
        {
            context.Add(file);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<FileDTO>> GetAllAsync(Expression<Func<FileEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            var query = context.Set<FileEntity>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query
                .Select(file => new FileDTO
                {
                    Id = file.Id,
                    CreatedAt = file.CreatedAt,
                    FileName = file.FileName,
                    SizeBytes = file.SizeBytes,
                    ContainerName = file.ContainerName,
                    BlobPath = file.BlobPath,
                    ContentType = file.ContentType,
                    Checksum = file.Checksum
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<FileDTO?> GetByFileNameAsync(string fileName, CancellationToken cancellationToken = default)
        {
            return await context.Set<FileEntity>()
                .FromSqlRaw("SELECT TOP 1 * FROM dbo.Files WHERE FileName LIKE {0}", fileName)
                .Select(file => new FileDTO
                {
                    Id = file.Id,
                    CreatedAt = file.CreatedAt,
                    FileName = file.FileName,
                    SizeBytes = file.SizeBytes,
                    ContainerName = file.ContainerName,
                    BlobPath = file.BlobPath,
                    ContentType = file.ContentType,
                    Checksum = file.Checksum
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<FileDTO?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken = default)
        {
            return await context.Set<FileEntity>()
                .FromSqlRaw("SELECT TOP 1 * FROM dbo.Files WHERE Id = {0}", fileId)
                .Select(file => new FileDTO
                {
                    Id = file.Id,
                    CreatedAt = file.CreatedAt,
                    FileName = file.FileName,
                    SizeBytes = file.SizeBytes,
                    ContainerName = file.ContainerName,
                    BlobPath = file.BlobPath,
                    ContentType = file.ContentType,
                    Checksum = file.Checksum
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
