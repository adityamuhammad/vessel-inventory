using System.Windows;
using VesselInventory.Services;
using VesselInventory.Utility;
using Unity;

namespace VesselInventory.ViewModel
{
    public abstract class ViewModelBase : ObservableObject
    {
        public virtual string Title { get; set; } = "Form";
        public RelayCommand<IClosable> CloseCommand { get; private set; }
        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public RelayCommand SettingCommand { get; private set; }
        public ViewModelBase()
        {
            CloseCommand = new RelayCommand<IClosable>(CloseWindow);
            LogoutCommand = new RelayCommand<IClosable>(CloseWindow);
            SettingCommand = new RelayCommand(SettingAction);
        }

        private void SettingAction(object obj)
        {
            var container = ((App)Application.Current).UnityContainer;
            Navigate.To(container.Resolve<SettingsVM>());
        }

        protected void CloseWindow(IClosable window)
        {
            if (window != null) window.Close();
        }
    }
}
