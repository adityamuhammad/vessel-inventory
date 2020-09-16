using System.IO;

namespace VesselInventory.Services
{
    public interface IOService
    {
        string OpenFileDialog();
        Stream OpenFile(string path);
    }
}
