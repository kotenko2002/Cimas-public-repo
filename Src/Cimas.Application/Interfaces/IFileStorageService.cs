using Cimas.Domain.Models;
using ErrorOr;

namespace Cimas.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<ErrorOr<string>> UploadFileAsync(MemoryStream stream, string fileName, string contentType);
        Task<ErrorOr<FileDownloadResult>> DownloadFileAsync(string fileId);
        Task<ErrorOr<Success>> DeleteFilesAsync(string fileId);
    }
}
