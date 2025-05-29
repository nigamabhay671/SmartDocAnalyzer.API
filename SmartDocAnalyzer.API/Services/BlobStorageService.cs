using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using SmartDocAnalyzer.API.Models;
using SmartDocAnalyzer.API.Services.Interfaces;

namespace SmartDocAnalyzer.API.Services
{
    public class BlobStorageService: IBlobStorageService
    {
      
        private readonly AzureBlobSettings _settings;
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IOptions<AzureBlobSettings> settings)
        {
            _settings = settings.Value;
            var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }

        public Uri GenerateBlobSasUri(string blobUrl)
        {
            // Parse the blob URL to get the blob name
            var blobUri = new Uri(blobUrl);
            var blobName = Path.GetFileName(blobUri.LocalPath);

            // Create a BlobServiceClient using your storage account credentials
            var blobServiceClient = new BlobServiceClient(new Uri(""), new StorageSharedKeyCredential("", ""));

            // Get a reference to the container and blob
            var containerClient = blobServiceClient.GetBlobContainerClient("documents");
            var blobClient = containerClient.GetBlobClient(blobName);

            // Check if the BlobClient is authorized with Shared Key
            if (blobClient.CanGenerateSasUri)
            {
                // Define the SAS token parameters
                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                // Set the permissions for the SAS
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // Generate the SAS URI
                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                return sasUri;
            }
            else
            {
                throw new InvalidOperationException("BlobClient is not authorized with Shared Key credentials.");
            }
        }
    }
}
