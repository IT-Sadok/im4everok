
using System.Collections.Concurrent;

using Application.Interfaces.MessageBrokers;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.MessageBrokers
{
    internal class AzureServiceBusPublisher : IPublisher
    {
        private readonly AzureServiceBusOptions _options;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ILogger<AzureServiceBusPublisher> _logger;
        private readonly ConcurrentDictionary<string, ServiceBusSender> _senders;

        public AzureServiceBusPublisher(IOptions<AzureServiceBusOptions> optionsObject,
            ILogger<AzureServiceBusPublisher> logger)
        {
            _options = optionsObject.Value;
            _serviceBusClient = new ServiceBusClient(_options.ConnectionString);
            _logger = logger;

            _senders = new();
        }

        public async Task<PublishResult> Publish(string queueOrTopic, string jsonMessageContent, string? messageId = null, CancellationToken ct = default)
        {
            ServiceBusSender sender = await GetOrCreateSenderAsync(queueOrTopic);

            try
            {
                ServiceBusMessage busMessage = CreateMessage(jsonMessageContent, messageId);

                await sender.SendMessageAsync(busMessage, ct);
                return new PublishResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ServiceBus publish message failed, queueOrTopic: {queueOrTopic}", queueOrTopic);
                return new PublishResult
                {
                    IsSuccess = false,
                    Reason = ex.Message
                };
            }
        }

        public async Task<PublishResult> PublishBatch(string queueOrTopic, IEnumerable<BatchedMessage> messages, CancellationToken cancellationToken = default)
        {
            if (!messages.Any()) return new PublishResult { IsSuccess = false };

            ServiceBusSender sender = await GetOrCreateSenderAsync(queueOrTopic);

            try
            {
                ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync(cancellationToken);
                foreach (BatchedMessage message in messages)
                {
                    ServiceBusMessage busMessage = CreateMessage(message.JsonContent, message.Id);

                    bool wasAdded = batch.TryAddMessage(busMessage);
                    if (!wasAdded)
                    {
                        var size = busMessage.Body.Length;
                        _logger.LogError("ServiceBus message exceeded size limit (batch or message itself). TopicOrQueue: {queueOrTopic} Message size: {size}, MessageId: {messageId}",
                            queueOrTopic, size, message.Id);

                        return new PublishResult
                        {
                            IsSuccess = false,
                            Reason = $"Failed to publish message with Id: {message.Id} to topicOrQueue: {queueOrTopic}"
                        };
                    }
                }

                await sender.SendMessagesAsync(batch, cancellationToken);
                return new PublishResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ServiceBus publish batch failed, queueOrTopic: {queueOrTopic}", queueOrTopic);
                return new PublishResult() { IsSuccess = false, Reason = ex.Message };
            }
        }
        private ServiceBusMessage CreateMessage(string jsonMessageContent, string? messageId = null)
        {
            BinaryData messageBody = BinaryData.FromString(jsonMessageContent);
            ServiceBusMessage busMessage = new()
            {
                MessageId = messageId,
                ContentType = "application/json",
                Body = messageBody
            };

            return busMessage;
        }

        private async Task<ServiceBusSender> GetOrCreateSenderAsync(string queueOrTopic)
        {
            if (_senders.TryGetValue(queueOrTopic, out var existingSender)) return existingSender;

            await _semaphore.WaitAsync();
            try
            {
                if (_senders.TryGetValue(queueOrTopic, out existingSender)) return existingSender;

                var sender = _serviceBusClient.CreateSender(queueOrTopic);
                _senders[queueOrTopic] = sender;
                return sender;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var sender in _senders.Values)
            {
                await sender.DisposeAsync();
            }
            _senders.Clear();

            await _serviceBusClient.DisposeAsync();
            _semaphore.Dispose();
        }
    }
}
