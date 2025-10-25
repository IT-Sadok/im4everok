using BLL.Services;
using BLL.Validators;

using DAL.Database;
using DAL.Models;
using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;

using LibraryManagement;

IDatabase<Book> database = new JsonDatabase("database.json");
IBookRepository bookRepository = new BookRepository(database);
IBookValidator addedBookValidator = new AddedBookValidator();
IBookService bookService = new BookService(bookRepository, addedBookValidator);

var consoleUI = new ConsoleUI(bookService);
LoadTest loadTest = new(bookService);

var strategies = new List<LibraryManagement.Strategies.AppStrategy>
{
    new LibraryManagement.Strategies.AppStrategy(1, "Run Console UI", consoleUI.Run),
    new LibraryManagement.Strategies.AppStrategy(2, "Run Load Test", loadTest.Run)
};

var application = new LibraryManagement.Strategies.Application(strategies);
await application.Run();