﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
