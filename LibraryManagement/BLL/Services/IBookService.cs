using BLL.DTOs;

using DAL.DTO;

namespace BLL.Services
{
    public interface IBookService
    {
        public Task<int> AddBook(AddBookRequest addBookRequest);
        public Task<bool> DeleteBook(int bookId);
        public Task<List<BookResponse>> SearchByAuthorOrName(string searchTerm);
        public Task<List<BookResponse>> GetAll();
        public Task<bool> RentBook(int bookId);
        public Task<bool> ReturnBook(int bookId);
        public Task<List<BookResponse>> GetAvailableBooks();
        public Task<List<BookResponse>> GetBorrowedBooks();
    }
}
