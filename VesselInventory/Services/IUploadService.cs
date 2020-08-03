using System;
using System.IO;

namespace VesselInventory.Services
{
    public interface IUploadService
    {
        bool UploadFile(string localPath, string targetDirectoryPath);
        string GetUploadedPath();
    }

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
                fileName = Path.GetFileName(localPath);
                var fileNameUploaded = string.Format("{0}_{1}", DateTime.Now.Ticks, fileName);
                var uploadPath = Path.Combine(targetDirectoryPath, fileNameUploaded);
                File.Copy(localPath, uploadPath);
                _uploadedPath = fileNameUploaded;
                return true;
            }
            return false;
        }
    }
}
