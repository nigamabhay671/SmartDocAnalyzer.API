namespace SmartDocAnalyzer.API.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        public Uri GenerateBlobSasUri(string url);   
    }
}
