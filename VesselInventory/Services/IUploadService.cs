using System;
using System.IO;

namespace VesselInventory.Services
{
    public interface IUploadService
    {
        bool UploadFile(string localPath, string targetDirectoryPath);
        string GetUploadedPath();
    }

}
