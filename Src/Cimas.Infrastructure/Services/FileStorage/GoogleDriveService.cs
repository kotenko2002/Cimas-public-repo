using Cimas.Application.Interfaces;
using Cimas.Domain.Models;
using ErrorOr;
using Google.Apis.Drive.v3;
using Microsoft.Extensions.Options;

namespace Cimas.Infrastructure.Services.FileStorage
{
    public class GoogleDriveService : IFileStorageService
    {
        private readonly GoogleDriveConfig _config;
        private readonly DriveService _service;

        public GoogleDriveService(IOptions<GoogleDriveConfig> googleDriveOptions, DriveService service)
        {
            _config = googleDriveOptions.Value;
            _service = service;
        }

        public async Task<ErrorOr<string>> UploadFileAsync(MemoryStream stream, string fileName, string contentType)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    MimeType = contentType,
                    Parents = new List<string> { _config.ParentFolderId }
                };
                FilesResource.CreateMediaUpload request;

                request = _service.Files.Create(fileMetadata, stream, contentType);
                request.Fields = "id";
                await request.UploadAsync();

                string fileId = request.ResponseBody.Id;

                return fileId;
            }
            catch (Exception)
            {
                return Error.Failure("Failed to upload the file");
            }
        }

        public async Task<ErrorOr<FileDownloadResult>> DownloadFileAsync(string fileId)
        {
            try
            {
                var request = _service.Files.Get(fileId);
                request.Fields = "id, name, mimeType";

                var file = await request.ExecuteAsync();

                var memoryStream = new MemoryStream();
                await request.DownloadAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return new FileDownloadResult()
                {
                    Stream = memoryStream,
                    FileName = file.Name,
                    ContentType = file.MimeType
                };
            }
            catch (Exception)
            {
                return Error.Failure("Failed to download the file");
            }
        }

        public async Task<ErrorOr<Success>> DeleteFilesAsync(string fileId)
        {
            try
            {
                await _service.Files.Delete(fileId).ExecuteAsync();

                return Result.Success;
            }
            catch (Exception)
            {
                return Error.Failure("Failed to delete the file");
            }
        }
    }
}
