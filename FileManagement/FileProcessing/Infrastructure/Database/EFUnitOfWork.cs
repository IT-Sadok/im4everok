
using Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    internal class EFUnitOfWork(DbContext dbContext) : IUnitOfWork
    {
        public async Task SaveChanges()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
