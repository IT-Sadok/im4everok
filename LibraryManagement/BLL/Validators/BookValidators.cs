using DAL.DTO;

namespace BLL.Validators
{
    public static class BookValidators
    {
        public static bool IsValidBookName(this AddBookRequest book)
        {
            return !string.IsNullOrWhiteSpace(book.Name) && book.Name.Length <= 200;
        }
        public static bool IsValidAuthorName(this AddBookRequest book)
        {
            return !string.IsNullOrWhiteSpace(book.Author) && book.Author.Length <= 100;
        }
        public static bool IsValidYearOfPublish(this AddBookRequest book)
        {
            int currentYear = DateTime.Now.Year;
            return book.YearOfPublish > 0 && book.YearOfPublish <= currentYear;
        }
    }
}
