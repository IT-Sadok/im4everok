using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options
{
    public class FileTextExtractionOptions
    {
        public const string SectionName = "DocumentIntelligence";

        [Required(ErrorMessage = "Document Intelligence API key is required.")]
        [MinLength(10)]
        public string ApiKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Document Intelligence endpoint is required.")]
        [Url]
        public string Endpoint { get; set; } = string.Empty;
    }
}
