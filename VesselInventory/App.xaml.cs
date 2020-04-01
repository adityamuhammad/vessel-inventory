using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using VesselInventory.Services;

namespace VesselInventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IUnityContainer _container;
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
    }
}
