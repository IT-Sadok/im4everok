using DAL.Database;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementation
{
    public class BookRepository(IDatabase<Book> db) : IBookRepository
    {
        public async Task<int> Add(Book book)
        {
            List<Book> allBooks = await GetAll();

            // Generate ID - this is data layer concern (like auto-increment)
            book.Id = allBooks.Count > 0 ? allBooks.Max(b => b.Id) + 1 : 1;

            allBooks.Add(book);
            await db.SaveData(allBooks);
            return book.Id;
        }

        public async Task<bool> Update(Book book)
        {
            List<Book> allBooks = await GetAll();
            var index = allBooks.FindIndex(b => b.Id == book.Id);

            if (index == -1)
                return false;

            allBooks[index] = book;
            await db.SaveData(allBooks);
            return true;
        }

        public async Task<bool> Delete(int bookId)
        {
            List<Book> allBooks = await GetAll();
            var initialCount = allBooks.Count;
            var newBooks = allBooks.Where(b => b.Id != bookId).ToList();

            if (initialCount == newBooks.Count)
                return false;

            await db.SaveData(newBooks);
            return true;
        }

        public async Task<Book?> GetById(int bookId)
        {
            List<Book> allBooks = await GetAll();
            return allBooks.FirstOrDefault(b => b.Id == bookId);
        }

        public async Task<List<Book>> GetAll()
        {
            return await db.GetAll();
        }

        public async Task<List<Book>> Search(Func<Book, bool> predicate)
        {
            List<Book> allBooks = await GetAll();
            return allBooks.Where(predicate).ToList();
        }
    }
}
