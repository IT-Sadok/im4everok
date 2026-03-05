namespace Application.Interfaces.Repositories
{
    public interface IOutboxEventRepository
    {
        Task Add(Domain.Entities.OutboxEvent outboxEvent);
        Task<List<Domain.Entities.OutboxEvent>> GetUnprocessedEventsAsync(int amount, string? type, CancellationToken ct);
        Task MarkAsSucceeded(IEnumerable<Guid> eventIds);
        Task MarkAsFailed(Dictionary<Guid, string> errorsByEventId);
    }
}
