



using Application.Interfaces.Repositories;

using Domain.Entities;

using Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class OutboxEventRepository(AppDbContext dbContext) : IOutboxEventRepository
    {
        public Task Add(OutboxEvent outboxEvent)
        {
            dbContext.Add(outboxEvent);
            return Task.CompletedTask;
        }

        public async Task<List<OutboxEvent>> GetUnprocessedEventsAsync(int amount, string? type)
        {
            var query = dbContext.Set<OutboxEvent>()
                .Where(e => e.ProcessedOnUtc == null);

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(e => e.Type == type);
            }

            return await query
                .Take(amount)
                .OrderBy(e => e.OccuredOnUtc)
                .ToListAsync();
        }

        public async Task MarkAsFailed(Dictionary<Guid, string> errorsByEventId)
        {
            await dbContext.Set<OutboxEvent>()
                .Where(e => errorsByEventId.Keys.Contains(e.Id))
                .ExecuteUpdateAsync(e =>
                {
                    e.SetProperty(ev => ev.Error, ev => errorsByEventId[ev.Id])
                     .SetProperty(ev => ev.RetryCount, ev => ev.RetryCount + 1);
                });
        }

        public async Task MarkAsSucceeded(IEnumerable<Guid> eventIds)
        {
            await dbContext.Set<OutboxEvent>()
                .Where(e => eventIds.Contains(e.Id))
                .ExecuteUpdateAsync(e =>
                {
                    e.SetProperty(ev => ev.ProcessedOnUtc, ev => DateTime.UtcNow);
                });
        }
    }
}
