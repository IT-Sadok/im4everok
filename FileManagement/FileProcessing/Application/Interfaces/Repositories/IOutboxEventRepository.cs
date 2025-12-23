namespace Application.Interfaces.Repositories
{
    public interface IOutboxEventRepository
    {
        Task Add(Domain.Entities.OutboxEvent outboxEvent);
    }
}
