using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VesselInventory.Services
{
    public interface IWindowService
    {
        void ShowWindow<T>(object ViewModel) where T: Window, new();
    }
    public class WindowService : IWindowService
    {
        public void ShowWindow<T>(object ViewModel) where T: Window, new()
        {
            T window = new T();
            window.DataContext = ViewModel;
            window.ShowDialog();
        }
    }
}
