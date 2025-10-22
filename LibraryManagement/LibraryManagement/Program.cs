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
await consoleUI.Run();