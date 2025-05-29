namespace SmartDocAnalyzer.API.Models
{
    public class AzureBlobSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = "documents";
    }
}
