using System.Windows;
using Unity;

namespace VesselInventory.Services
{
    public interface IWindowService
    {
        void ShowDialogWindow<T>(object ViewModel) where T: Window, new();
        void ShowWindow<T>(object ViewModel) where T: Window, new();
    }
    public class WindowService : IWindowService
    {
        private IUnityContainer container = ((App)Application.Current).UnityContainer;
        public void ShowDialogWindow<T>(object ViewModel) where T: Window, new()
        {
            var window = container.Resolve<T>();
            window.DataContext = ViewModel;
            window.ShowDialog();
        }
        public void ShowWindow<T>(object ViewModel) where T: Window, new()
        {
            var window = container.Resolve<T>();
            window.DataContext = ViewModel;
            window.Show();
        }
    }
}
