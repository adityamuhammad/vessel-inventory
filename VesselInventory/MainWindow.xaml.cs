using System.Windows;
using System.Windows.Controls;
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
                    DataContext = new VesselGoodReceiveViewModel();
                    break;
                case 3:
                    break;
            }
        }

        private void ShowAndMinimizeNavBar(object sender, RoutedEventArgs e)
        {
            int uid = int.Parse(((Button)e.Source).Uid);
            switch (uid)
            {
                case 0:
                    ExpandMenuBtn.Visibility = Visibility.Visible;
                    MinimizeMenuBtn.Visibility = Visibility.Collapsed;
                    NavBarColumnMenu.Width = new GridLength(45, GridUnitType.Pixel);
                    break;
                case 1:
                    ExpandMenuBtn.Visibility = Visibility.Collapsed;
                    MinimizeMenuBtn.Visibility = Visibility.Visible;
                    NavBarColumnMenu.Width = new GridLength(180, GridUnitType.Pixel);
                    break;
                default:
                    break;
            }
        }
    }

}
