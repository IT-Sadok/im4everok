namespace Domain.Entities
{
    public class OutboxEvent
    {
        public Guid Id { get; set; }
        public DateTime OccuredOnUtc { get; set; }
        public string Type { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime? ProcessedOnUtc { get; set; }
        public string? Error { get; set; }
        public int RetryCount { get; set; }
    }
}
