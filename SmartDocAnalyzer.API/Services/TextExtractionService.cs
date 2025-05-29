using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using Microsoft.Extensions.Options;
using SmartDocAnalyzer.API.Models;
using SmartDocAnalyzer.API.Services.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace SmartDocAnalyzer.API.Services
{
    public class TextExtractionService:ITextExtractionService
    {
        private readonly AzureFormRecognizerSettings _settings;

        public TextExtractionService(IOptions<AzureFormRecognizerSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> ExtractTextFromUrlAsync(string fileUrl)
        {
            var client = new DocumentAnalysisClient(new Uri(_settings.Endpoint), new AzureKeyCredential(_settings.ApiKey));
            var operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-document", new Uri(fileUrl));
            var result = operation.Value;

            var allText = new System.Text.StringBuilder();

            foreach (var page in result.Pages)
            {
                foreach (var line in page.Lines)
                {
                    allText.AppendLine(line.Content);
                }
            }

            return allText.ToString();
        }        
    }
}
