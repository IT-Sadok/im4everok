using DAL.Enums;

namespace DAL.Models
{
    //назва, автор, рік випуску, унікальний код книги.
    // thought to name it like "BookRecord" but Book works too i guess
    public class Book
    {
        // unique code - lets make it integer for simplicity, would be guid irl
        public int Id { get; set; } = 0;

        public string Name { get; set; }
        
        // can be made as FirstName/LastName - i've left it as a single string for simplicity
        public string Author { get; set; }

        // i would make this a date time for sure, but requirements say "year" - so i use number
        public int YearOfPublish { get; set; }

        public BookState State { get; set; } = BookState.Available;
    }
}
