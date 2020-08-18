using System.Windows;
using System.Windows.Controls;
using System;
using Unity;
using VesselInventory.ViewModel;
using VesselInventory.Services;

namespace VesselInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IClosable
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void MenuChanged(object sender, RoutedEventArgs e)
        {
            int indexMenu = ListViewMenu.SelectedIndex;
            var container = ((App)Application.Current).UnityContainer;
            switch (indexMenu)
            {
                case 0:
                    DataContext = container.Resolve<HomeVM>();
                    break;
                case 1:
                    DataContext = container.Resolve<RequestFormVM>();
                    break;
                case 2:
                    DataContext = container.Resolve<VesselGoodReceiveVM>();
                    break;
                case 3:
                    DataContext = container.Resolve<VesselGoodIssuedVM>();
                    break;
                case 4:
                    DataContext = container.Resolve<VesselGoodReturnVM>();
                    break;
                case 5:
                    DataContext = container.Resolve<OnHandVM>();
                    break;
                default:
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
