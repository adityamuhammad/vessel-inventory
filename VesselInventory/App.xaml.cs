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
using VesselInventory.Views;

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
                    _container = new UnityContainer();
                return _container;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UnityContainer.RegisterType<IWindowService, WindowService>();
            UnityContainer.RegisterType<IOService, OpenPdfFileDialog>();
            UnityContainer.RegisterType<IUploadService, UploadService>();
            UnityContainer.RegisterType(typeof(IGenericRepository<>), typeof(GenericRepository<>), new TransientLifetimeManager());
            UnityContainer.RegisterType<IRequestFormRepository, RequestFormRepository>();
            UnityContainer.RegisterType<IRequestFormItemRepository, RequestFormItemRepository>();
            UnityContainer.RegisterType<IVesselGoodReceiveRepository, VesselGoodReceiveRepository>();
            UnityContainer.RegisterType<IVesselGoodReceiveItemRejectRepository, VesselGoodReceiveItemRejectRepository>();
            UnityContainer.RegisterType<IVesselGoodReceiveItemRepository, VesselGoodReceiveItemRepository>();
            UnityContainer.RegisterType<IVesselGoodIssuedRepository, VesselGoodIssuedRepository>();
            UnityContainer.RegisterType<IVesselGoodIssuedItemRepository, VesselGoodIssuedItemRepository>();
            UnityContainer.RegisterType<IVesselGoodReturnRepository, VesselGoodReturnRepository>();
            UnityContainer.RegisterType<IVesselGoodReturnItemRepository, VesselGoodReturnItemRepository>();
            UnityContainer.RegisterType<IOnHandRepository, OnHandRepository>();

            Window window = UnityContainer.Resolve<Login_View>();
            window.DataContext = UnityContainer.Resolve<LoginVM>();
            window.Show();
        }
    }
}
