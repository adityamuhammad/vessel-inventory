using System;
using System.IO;
using System.Windows.Forms;

namespace VesselInventory.Services.Impl
{
    public class OpenPdfFileDialog : IOService
    {
        public OpenPdfFileDialog() { }
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
                    return null;
                }
            }
        }
    }
}
