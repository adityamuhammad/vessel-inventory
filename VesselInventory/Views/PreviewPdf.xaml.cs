using System;
using System.Windows;
namespace VesselInventory.Views
{
    /// <summary>
    /// Interaction logic for PreviewPdf.xaml
    /// </summary>
    public partial class PreviewPdf : Window
    {
        public PreviewPdf()
        {
            InitializeComponent();
        }

        public void SetAttachment(string attachmentLocation)
        {
            attachmentLocation = attachmentLocation.Replace("\\\\", "\\");
            PreviewPdfTemplate.Navigate(new Uri(attachmentLocation));
        }
    }
}
