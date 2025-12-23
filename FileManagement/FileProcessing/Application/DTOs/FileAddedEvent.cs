namespace Application.DTOs
{
    public class FileAddedEvent
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; } = default!;
        public long SizeBytes { get; set; }
        public string ContentType { get; set; } = default!;
        public string BlobPath { get; set; } = default!;
        public string ContainerName { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
    }
}
