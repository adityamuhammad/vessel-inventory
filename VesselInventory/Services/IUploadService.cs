﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                _uploadedPath = Path.Combine(
                    targetDirectoryPath, 
                    DateTime.Now.ToString("yyyy-MM-dd")
                    + '_' + DateTime.Now.Ticks 
                    + '_' +  fileName);

                File.Copy(localPath, _uploadedPath);
                return true;
            }
            return false;
        }
    }
}
