using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VesselInventory.Utility
{
    public static class Navigate
    {
        public static void To(object viewModel)
        {
            Application.Current.Windows[0].DataContext = viewModel ;
        }
    }
}
