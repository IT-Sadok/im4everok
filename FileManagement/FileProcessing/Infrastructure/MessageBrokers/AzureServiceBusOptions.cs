using System.ComponentModel.DataAnnotations;

namespace Infrastructure.MessageBrokers
{
    internal class AzureServiceBusOptions
    {
        public const string SectionName = "ServiceBus";

        [Required(ErrorMessage = "Service Bus connection string is required.")]
        [MinLength(10)]
        public string ConnectionString { get; set; } = string.Empty;
    }
}
