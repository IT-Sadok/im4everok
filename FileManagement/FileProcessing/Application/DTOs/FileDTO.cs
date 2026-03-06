namespace Application.DTOs
{
    public class FileDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string FileName { get; set; } = default!;
        public long SizeBytes { get; set; }

        public string ContainerName { get; set; } = default!;
        public string BlobPath { get; set; } = default!;

        public string ContentType { get; set; } = default!;
        public string Checksum { get; set; } = default!;

        public string? SASUrl { get; set; }
    }
}
