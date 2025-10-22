using BLL.DTOs;
using BLL.Validators;

using DAL.DTO;
using DAL.Enums;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    // can also make bookValidator a collection -
    // so we could reuse validations in many places but i dont see a point in this rn
    public class BookService(IBookRepository bookRepository, IBookValidator bookValidator) : IBookService
    {
        public async Task<int> AddBook(AddBookRequest book)
        {
            bookValidator.ValidateAddedBook(book);

            Book bookEntity = new()
            {
                Name = book.Name,
                Author = book.Author,
                YearOfPublish = book.YearOfPublish,
                State = BookState.Available
            };

            return await bookRepository.Add(bookEntity);
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            // is it worth to check if book exists first? sometimes business wants to check it, but on
            // my current project its ok to just say "yeah, its deleted cause it doesnt exist anyway"
            return await bookRepository.Delete(bookId);
        }

        public async Task<List<BookResponse>> GetAll()
        {
            var books = await bookRepository.GetAll();
            return books.Select(MapToResponse).ToList();
        }

        public async Task<List<BookResponse>> GetAvailableBooks()
        {
            var books = await bookRepository.Search(b => b.State == BookState.Available);
            return books.Select(MapToResponse).ToList();
        }

        public async Task<List<BookResponse>> GetBorrowedBooks()
        {
            var books = await bookRepository.Search(b => b.State == BookState.Borrowed);
            return books.Select(MapToResponse).ToList();
        }

        public async Task<bool> RentBook(int bookId)
        {
            var book = await bookRepository.GetById(bookId);

            if (book is null)
                throw new ArgumentException("Book not found.");

            if (book.State == BookState.Borrowed)
                throw new InvalidOperationException("Book is already rented.");

            book.State = BookState.Borrowed;
            return await bookRepository.Update(book);
        }

        public async Task<bool> ReturnBook(int bookId)
        {
            var book = await bookRepository.GetById(bookId);

            if (book is null)
                throw new ArgumentException("Book not found.");

            if (book.State == BookState.Available)
                throw new InvalidOperationException("Book is not rented.");

            book.State = BookState.Available;
            return await bookRepository.Update(book);
        }

        public async Task<List<BookResponse>> SearchByAuthorOrName(string searchTerm)
        {
            var books = await bookRepository.Search(b =>
                b.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            return books.Select(MapToResponse).ToList();
        }

        // Private mapping method
        private static BookResponse MapToResponse(Book book)
        {
            return new BookResponse
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                YearOfPublish = book.YearOfPublish,
                Status = book.State.ToString(),
                IsAvailable = book.State == BookState.Available
            };
        }
    }
}
