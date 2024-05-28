namespace Cimas.Infrastructure.Services.FileStorage
{
    public class GoogleDriveConfig
    {
        public string PrivateKey { get; set; }
        public string ClientEmail { get; set; }
        public string ApplicationName { get; set; }
        public string ParentFolderId { get; set; }
    }
}
