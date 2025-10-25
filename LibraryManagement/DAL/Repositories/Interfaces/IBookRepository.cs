using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IBookRepository
    {
        public Task<int> Add(Book book);
        public Task<bool> Update(Book book);
        public Task<bool> Delete(int bookId);
        public Task<Book?> GetById(int bookId);
        public Task<List<Book>> GetAll();
        public Task<List<Book>> Search(Func<Book, bool> predicate);
    }
}
