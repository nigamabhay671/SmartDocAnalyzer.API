using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using SmartDocAnalyzer.API.Services;
using SmartDocAnalyzer.API.Services.Interfaces;

namespace SmartDocAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ITextExtractionService _textExtractionService;
        BlobStorageService BlobStorageService;

        public DocumentController(IBlobStorageService blobStorageService,ITextExtractionService textExtractionService)
        {
            _blobStorageService = blobStorageService;
            _textExtractionService = textExtractionService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var fileUrl = await _blobStorageService.UploadFileAsync(file);

            return Ok(new
            {
                Message = "Upload successful",
                FileName = file.FileName,
                Url = fileUrl
            });
        }

        [HttpPost("upload-and-extract")]
        public async Task<IActionResult> UploadAndExtract(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var fileUrl = await _blobStorageService.UploadFileAsync(file);
            // var generateSASUri = BlobStorageService.GenerateBlobSasUri();
            var sasUri = _blobStorageService.GenerateBlobSasUri(fileUrl);

            var extractedText = await _textExtractionService.ExtractTextFromUrlAsync(sasUri.ToString());

            return Ok(new
            {
                Message = "File uploaded and analyzed successfully",
                FileUrl = fileUrl,
                ExtractedText = extractedText
            });
        }

       



    }
}
