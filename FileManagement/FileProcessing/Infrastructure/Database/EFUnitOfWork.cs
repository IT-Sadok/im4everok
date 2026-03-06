using Application.Interfaces;

namespace Infrastructure.Database
{
    internal class EFUnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        public async Task<ITransaction> BeginTransactionAsync()
        {
            var transaction = await dbContext.Database.BeginTransactionAsync();
            return new EFTransaction(transaction);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
