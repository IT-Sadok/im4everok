using DAL.DTO;
using DAL.Models;

namespace BLL.Services
{
    public interface IBookService
    {
        public Task<int> AddBook(AddBookRequest addBookRequest);
        public Task<bool> DeleteBook(int bookId);
        public Task<List<Book>> SearchByAuthorOrName(string searchTerm);
        public Task<List<Book>> GetAll();
        public Task<bool> RentBook(int bookId);
        public Task<bool> ReturnBook(int bookId);
        public Task<List<Book>> GetAvailableBooks();
        public Task<List<Book>> GetBorrowedBooks();
    }
}
