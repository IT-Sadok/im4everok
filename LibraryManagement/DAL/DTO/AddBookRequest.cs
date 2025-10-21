namespace DAL.DTO
{
    public record AddBookRequest(
        string Name,
        string Author,
        int YearOfPublish
    );
}
