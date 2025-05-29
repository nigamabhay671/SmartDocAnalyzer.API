namespace SmartDocAnalyzer.API.Services.Interfaces
{
    public interface ITextExtractionService
    {
        Task<string> ExtractTextFromUrlAsync(string fileUrl);
    }
}
