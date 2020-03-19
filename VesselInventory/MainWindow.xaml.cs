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

namespace VesselInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }
        private void MenuChanged(object sender, RoutedEventArgs e)
        {
            int indexMenu = ListViewMenu.SelectedIndex;
            switch (indexMenu)
            {
                case 0:
                    DataContext = new HomeViewModel();
                    break;
                case 1:
                    DataContext = new RequestFormViewModel();
                    break;
                case 2:
                    break;
            }
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
            NavBarCol.Width = new GridLength(180, GridUnitType.Pixel);
        }
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            NavBarCol.Width = new GridLength(45, GridUnitType.Pixel);
        }
    }

}
