using Application.Interfaces;

using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Database
{
    internal class EFUnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync() => _transaction = await dbContext.Database.BeginTransactionAsync();

        public async Task CommitAsync()
        {
            if (_transaction == null) return;

            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
