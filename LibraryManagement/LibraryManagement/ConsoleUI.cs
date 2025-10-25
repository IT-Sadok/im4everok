using BLL.DTOs;
using BLL.Services;

using DAL.DTO;

namespace LibraryManagement
{
    internal class ConsoleUI(IBookService bookService)
    {
        public async Task Run()
        {
            Console.WriteLine("=== Library Management System ===\n");

            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Add a book");
                Console.WriteLine("2. Delete a book");
                Console.WriteLine("3. Search by Author or Name");
                Console.WriteLine("4. Show all available books");
                Console.WriteLine("5. Rent a book");
                Console.WriteLine("6. Return a book");
                Console.WriteLine("7. Exit");

                Console.Write("\nEnter your choice (1-7): ");
                string? input = Console.ReadLine();

                int? parsed = int.TryParse(input, out int result) ? result : null;

                try
                {
                    switch (parsed)
                    {
                        case 1:
                            await AddBook();
                            break;
                        case 2:
                            await DeleteBook();
                            break;
                        case 3:
                            await SearchBooks();
                            break;
                        case 4:
                            await ShowAllBooks();
                            break;
                        case 5:
                            await RentBook();
                            break;
                        case 6:
                            await ReturnBook();
                            break;
                        case 7:
                            Console.WriteLine("\nThank you for using the Library Management System. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("\n❌ Invalid choice. Please enter a number between 1 and 7.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\n❌ Validation Error: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"\n❌ Operation Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Unexpected Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private async Task AddBook()
        {
            Console.WriteLine("\n=== Add New Book ===");

            Console.Write("Enter book name: ");
            string? name = Console.ReadLine();

            Console.Write("Enter author name: ");
            string? author = Console.ReadLine();

            Console.Write("Enter year of publish: ");
            string? yearInput = Console.ReadLine();

            if (!int.TryParse(yearInput, out int year))
            {
                Console.WriteLine("\n❌ Invalid year format.");
                return;
            }

            var addBookRequest = new AddBookRequest(name ?? string.Empty, author ?? string.Empty, year);

            int bookId = await bookService.AddBook(addBookRequest);
            Console.WriteLine($"\n✅ Book added successfully with ID: {bookId}");
        }

        private async Task DeleteBook()
        {
            Console.WriteLine("\n=== Delete Book ===");

            Console.Write("Enter book ID to delete: ");
            string? idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out int bookId))
            {
                Console.WriteLine("\n❌ Invalid ID format.");
                return;
            }

            bool deleted = await bookService.DeleteBook(bookId);

            if (deleted)
                Console.WriteLine($"\n✅ Book with ID {bookId} deleted successfully.");
            else
                Console.WriteLine($"\n⚠️ Book with ID {bookId} not found.");
        }

        private async Task SearchBooks()
        {
            Console.WriteLine("\n=== Search Books ===");

            Console.Write("Enter search term (author or book name): ");
            string? searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("\n❌ Search term cannot be empty.");
                return;
            }

            List<BookResponse> books = await bookService.SearchByAuthorOrName(searchTerm);

            if (books.Count == 0)
            {
                Console.WriteLine($"\n⚠️ No books found matching '{searchTerm}'.");
                return;
            }

            Console.WriteLine($"\n✅ Found {books.Count} book(s):");
            DisplayBooks(books);
        }

        private async Task ShowAllBooks()
        {
            Console.WriteLine("\n=== All Books ===");

            List<BookResponse> books = await bookService.GetAll();

            if (books.Count == 0)
            {
                Console.WriteLine("\n⚠️ No books in the library.");
                return;
            }

            Console.WriteLine($"\n📚 Total books: {books.Count}");
            DisplayBooks(books);
        }

        private async Task RentBook()
        {
            Console.WriteLine("\n=== Rent Book ===");

            List<BookResponse> availableBooks = await bookService.GetAvailableBooks();

            if (availableBooks.Count == 0)
            {
                Console.WriteLine("\n⚠️ No books available for rent.");
                return;
            }

            Console.WriteLine("\nAvailable books:");
            DisplayBooks(availableBooks);

            Console.Write("\nEnter book ID to rent: ");
            string? idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out int bookId))
            {
                Console.WriteLine("\n❌ Invalid ID format.");
                return;
            }

            bool rented = await bookService.RentBook(bookId);

            if (rented)
                Console.WriteLine($"\n✅ Book with ID {bookId} rented successfully.");
        }

        private async Task ReturnBook()
        {
            Console.WriteLine("\n=== Return Book ===");

            List<BookResponse> borrowedBooks = await bookService.GetBorrowedBooks();

            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("\n⚠️ No books are currently rented.");
                return;
            }

            Console.WriteLine("\nBorrowed books:");
            DisplayBooks(borrowedBooks);

            Console.Write("\nEnter book ID to return: ");
            string? idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out int bookId))
            {
                Console.WriteLine("\n❌ Invalid ID format.");
                return;
            }

            bool returned = await bookService.ReturnBook(bookId);

            if (returned)
                Console.WriteLine($"\n✅ Book with ID {bookId} returned successfully.");
        }

        private void DisplayBooks(List<BookResponse> books)
        {
            Console.WriteLine("\n" + new string('-', 100));
            Console.WriteLine($"{"ID",-5} | {"Title",-30} | {"Author",-25} | {"Year",-6} | {"Status",-10}");
            Console.WriteLine(new string('-', 100));

            foreach (var book in books)
            {
                string statusIcon = book.IsAvailable ? "✓" : "✗";

                Console.WriteLine($"{book.Id,-5} | {TruncateString(book.Name, 30),-30} | {TruncateString(book.Author, 25),-25} | {book.YearOfPublish,-6} | {statusIcon} {book.Status,-8}");
            }

            Console.WriteLine(new string('-', 100));
        }

        private string TruncateString(string text, int maxLength)
        {
            if (text.Length <= maxLength)
                return text;

            return text[..(maxLength - 3)] + "...";
        }
    }
}