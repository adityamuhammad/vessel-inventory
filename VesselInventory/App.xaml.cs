using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.ViewModel;

namespace VesselInventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;
        public IUnityContainer UnityContainer
        {
            get
            {
                if (_container == null)
                {
                    _container = new UnityContainer();

                }
                return _container;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            UnityContainer.RegisterType<IWindowService, WindowService>();
            UnityContainer.RegisterType<IRequestFormRepository, RequestFormRepository>();
            var homeViewModel = UnityContainer.Resolve<HomeViewModel>();
            Window window = UnityContainer.Resolve<MainWindow>();
            window.DataContext = homeViewModel;
            window.Show();
        }
    }
}
