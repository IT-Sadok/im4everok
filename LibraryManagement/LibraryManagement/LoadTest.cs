using BLL.Services;

using DAL.DTO;

namespace LibraryManagement
{
    internal class LoadTest(IBookService bookService)
    {
        internal async Task Run()
        {
            Task[] tasks = new Task[100];

            int addedAmount = 0, deletedAmount = 0;

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    try
                    {
                        int id = await bookService.AddBook(new AddBookRequest("Hello", "Book name", 1995));
                        Interlocked.Increment(ref addedAmount);

                        await bookService.GetAll();

                        bool wasDeleted = await bookService.DeleteBook(id);
                        if (wasDeleted)
                        {
                            Interlocked.Increment(ref deletedAmount);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"All done. Added: {addedAmount}, Deleted: {deletedAmount}");
        }
    }
}
