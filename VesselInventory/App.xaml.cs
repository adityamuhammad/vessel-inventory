using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
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
            UnityContainer.RegisterType<IOService, OpenPdfFileDialog>();
            UnityContainer.RegisterType<IUploadService, UploadService>();
            UnityContainer.RegisterType<IRequestFormRepository, RequestFormRepository>();
            UnityContainer.RegisterType<IRequestFormItemRepository, RequestFormItemRepository>();
            UnityContainer.RegisterType<IVesselGoodReceiveRepository, VesselGoodReceiveRepository>();
            UnityContainer.RegisterType<IVesselGoodReceiveItemRejectRepository, VesselGoodReceiveItemRejectRepository>();
            UnityContainer.RegisterType(typeof(IRepository<>), typeof(Repository<>), new TransientLifetimeManager());

            var homeViewModel = UnityContainer.Resolve<HomeVM>();
            Window window = UnityContainer.Resolve<MainWindow>();
            window.DataContext = homeViewModel;
            window.Show();
        }
    }
}
