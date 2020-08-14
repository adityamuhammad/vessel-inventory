
using System;
using System.Configuration;
using System.IO;

namespace VesselInventory.Services.Impl
{
    public class UploadService : IUploadService
    {
        private string _uploadedPath;

        public string GetUploadedPath()
        {
            return _uploadedPath;
        }

        public bool UploadFile(string localPath, string targetDirectoryPath)
        {
            string fileName = string.Empty;

            if (!Directory.Exists(targetDirectoryPath))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(targetDirectoryPath);
            }

            if (File.Exists(localPath))
            {
                var clientId = ConfigurationManager.AppSettings["ClientId"];
                fileName = Path.GetFileName(localPath);
                var fileNameUploaded = string.Format("{0}_{1}_{2}", clientId, DateTime.Now.Ticks, fileName);
                var uploadPath = Path.Combine(targetDirectoryPath, fileNameUploaded);
                File.Copy(localPath, uploadPath);
                _uploadedPath = fileNameUploaded;
                return true;
            }
            return false;
        }
    }
}
