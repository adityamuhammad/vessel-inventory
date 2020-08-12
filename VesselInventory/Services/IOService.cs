using System;
using System.IO;
using System.Windows.Forms;

namespace VesselInventory.Services
{
    public interface IOService
    {
        string OpenFileDialog();
        Stream OpenFile(string path);
    }
}
