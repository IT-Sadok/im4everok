namespace BLL.DTOs
{
    public record BookResponse
    {
        public int Id { get; init; }
    public required string Name { get; init; }
        public required string Author { get; init; }
        public int YearOfPublish { get; init; }
      public string Status { get; init; } = string.Empty;
        public bool IsAvailable { get; init; }
    }
}
