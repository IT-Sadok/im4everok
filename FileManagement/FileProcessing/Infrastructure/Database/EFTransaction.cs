using Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Database
{
    internal class EFTransaction(IDbContextTransaction transaction) : ITransaction
    {
        public async Task CommitAsync() => await transaction.CommitAsync();

        public async Task RollbackAsync() => await transaction.RollbackAsync();

        public async ValueTask DisposeAsync() => await transaction.DisposeAsync();
    }
}
