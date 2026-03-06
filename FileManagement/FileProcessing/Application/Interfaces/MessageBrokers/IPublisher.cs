namespace Application.Interfaces.MessageBrokers
{
    public interface IPublisher : IAsyncDisposable
    {
        Task<PublishResult> Publish(string queueOrTopic, string jsonMessageContent, string? messageId = null, CancellationToken ct = default);
        Task<PublishResult> PublishBatch(string queueOrTopic, IEnumerable<BatchedMessage> messages, CancellationToken cancellationToken = default);
    }

    public class BatchedMessage
    {
        public string? JsonContent { get; set; }
        public string? Id { get; set; }
    }

    public class PublishResult
    {
        public bool IsSuccess { get; set; }
        public string? Reason { get; set; }
    }
}
