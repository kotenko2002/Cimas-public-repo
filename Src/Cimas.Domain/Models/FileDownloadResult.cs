namespace Cimas.Domain.Models
{
    public class FileDownloadResult
    {
        public MemoryStream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
