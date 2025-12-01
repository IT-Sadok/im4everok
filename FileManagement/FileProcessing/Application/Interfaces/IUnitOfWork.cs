namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task BeginTransactionAsync();
    }
}
