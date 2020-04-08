using System.Windows;
using System.Windows.Controls;
using VesselInventory.ViewModel;
using Unity;
using Unity.Injection;

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
        }
        private void MenuChanged(object sender, RoutedEventArgs e)
        {
            int indexMenu = ListViewMenu.SelectedIndex;
            var container = ((App)Application.Current).UnityContainer;
            switch (indexMenu)
            {
                case 0:
                    DataContext = container.Resolve<HomeViewModel>();
                    break;
                case 1:
                    DataContext = container.Resolve<RequestFormViewModel>();
                    break;
                case 2:
                    DataContext = container.Resolve<VesselGoodReceiveViewModel>();
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
