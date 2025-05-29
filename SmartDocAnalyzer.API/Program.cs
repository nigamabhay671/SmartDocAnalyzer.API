
using SmartDocAnalyzer.API.Models;
using SmartDocAnalyzer.API.Services.Interfaces;
using SmartDocAnalyzer.API.Services;

namespace SmartDocAnalyzer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("Azure:Blob"));
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            builder.Services.Configure<AzureFormRecognizerSettings>(builder.Configuration.GetSection("Azure:FormRecognizer"));
            builder.Services.AddScoped<ITextExtractionService, TextExtractionService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
