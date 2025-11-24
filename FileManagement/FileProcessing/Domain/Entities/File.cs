namespace Domain.Entities
{
    public class FileEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FileName { get; set; }
        public float Size { get; set; }
    }
}
