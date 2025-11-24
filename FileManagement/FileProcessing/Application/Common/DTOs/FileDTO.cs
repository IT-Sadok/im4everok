namespace Application.Common.DTOs
{
    public class FileDTO
    {
        public Guid? Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? FileName { get; set; }
        public float Size { get; set; }
    }
}
