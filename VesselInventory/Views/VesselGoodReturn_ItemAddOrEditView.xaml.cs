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
using VesselInventory.Services;

namespace VesselInventory.Views
{
    /// <summary>
    /// Interaction logic for VesselGoodReturn_ItemAddOrEditView.xaml
    /// </summary>
    public partial class VesselGoodReturn_ItemAddOrEditView : Window, IClosable
    {
        public VesselGoodReturn_ItemAddOrEditView()
        {
            InitializeComponent();
        }
    }
}