using Cimas.Application.Interfaces;
using Cimas.Infrastructure;
using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System.Text;

namespace Cimas.IntegrationTests.HandlersTests
{
    public class FileStorageServiceTest
    {
        private IFileStorageService _fileStorageService;
        private string _fileId;

        private const string FileContentText = "Це приклад тексту, який буде збережений у MemoryStream.";
        private const string FileName = "test.txt";
        private const string FileType = "text/plain";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();
            app.Start();

            _fileStorageService = app.Services.GetService<IFileStorageService>();
        }

        [Test]
        public async Task FileStorageService_CrudOperations_Success()
        {
            await UploadFile();
            await DownloadFile();
            await DeleteFile();
        }

        private async Task UploadFile()
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(FileContentText));
            ErrorOr<string> uploadFileResult = await _fileStorageService.UploadFileAsync(memoryStream, FileName, FileType);

            Assert.That(uploadFileResult.IsError, Is.False);
            _fileId = uploadFileResult.Value;
        }

        private async Task DownloadFile()
        {
            var downloadFileResult = await _fileStorageService.DownloadFileAsync(_fileId);
            using MemoryStream receivedStream = downloadFileResult.Value.Stream;

            string receivedText = await new StreamReader(receivedStream).ReadToEndAsync();
            Assert.That(receivedText, Is.EqualTo(FileContentText));
            Assert.That(downloadFileResult.Value.FileName, Is.EqualTo(FileName));
            Assert.That(downloadFileResult.Value.ContentType, Is.EqualTo(FileType));
        }

        private async Task DeleteFile()
        {
            ErrorOr<Success> deleteFileResult = await _fileStorageService.DeleteFilesAsync(_fileId);
            Assert.That(deleteFileResult.IsError, Is.False);
        }
    }
}
