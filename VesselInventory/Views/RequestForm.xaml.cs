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
using VesselInventory.Utility;
using VesselInventory.ViewModel;

namespace VesselInventory.Views
{
    /// <summary>
    /// Interaction logic for RequestForm.xaml
    /// </summary>
    public partial class RequestForm : UserControl
    {
        public RequestForm()
        {
            InitializeComponent();
        }
        private void TabRequestForm_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursor.Margin = new Thickness((150 * index), 29, 0, 11);

            switch (index)
            {
                case 1:
                    Navigate.To(new RequestFormItemStatusViewModel());
                    break;
                case 2:
                    Navigate.To(new RequestFormItemPendingViewModel());
                    break;
                default:
                    break;
            }
        }

        private void RequestAction_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Opacity = 0.5;
            //Window AddNewForm = new RequestForm_ModalAdd();
            //AddNewForm.ShowDialog();
        }

    }

}
