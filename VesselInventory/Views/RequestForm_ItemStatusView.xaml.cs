using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VesselInventory.ViewModel;
using VesselInventory.Utility;

namespace VesselInventory.Views
{
    /// <summary>
    /// Interaction logic for RequestForm_ItemStatus.xaml
    /// </summary>
    public partial class RequestForm_ItemStatusView : UserControl
    {
        public RequestForm_ItemStatusView()
        {
            InitializeComponent();
        }

        private void TabRequestForm_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursor.Margin = new Thickness((150 * index), 29, 0, 11);

            switch (index)
            {
                case 0:
                    Navigate.To(new RequestFormViewModel());
                    break;
                case 2:
                    Navigate.To(new RequestFormItemPendingViewModel());
                    break;
                default:
                    break;
            }

        }
    }
}
