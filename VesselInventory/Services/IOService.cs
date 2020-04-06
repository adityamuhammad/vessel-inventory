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
    public sealed class OpenPdfFileDialog : IOService
    {
        public Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public string OpenFileDialog()
        {
            using (var dialog = new OpenFileDialog() { Filter = "Pdf Files| *.pdf" })
            {
                var result = dialog.ShowDialog();
                try
                {
                    if(result == DialogResult.OK)
                        return dialog.FileName;
                    throw new Exception();
                } catch
                {
                    return "Error";
                }
            }
        }
    }
}
