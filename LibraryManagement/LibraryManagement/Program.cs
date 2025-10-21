using BLL.Services;

using DAL.Database;
using DAL.Models;
using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;

using LibraryManagement;

IDatabase<Book> database = new JsonDatabase("database.json");
IBookRepository bookRepository = new BookRepository(database);
IBookService bookService = new BookService(bookRepository);

var consoleUI = new ConsoleUI(bookService);
await consoleUI.Run();