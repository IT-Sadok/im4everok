
using Application.Interfaces.Repositories;

using Domain.Entities;

using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    internal class OutboxEventRepository(AppDbContext dbContext) : IOutboxEventRepository
    {
        public Task Add(OutboxEvent outboxEvent)
        {
            dbContext.Add(outboxEvent);
            return Task.CompletedTask;
        }
    }
}
