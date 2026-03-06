namespace Infrastructure.FileStorage
{
    public class FileStorageConfiguration
    {
        public const string SectionName = "BlobStorage";

        public string ConnectionName { get; set; } = string.Empty;
        public int SasTokenExpiryMinutes { get; set; } = 1;
    }
}
