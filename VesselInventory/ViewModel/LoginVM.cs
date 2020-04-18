using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Unity;
using VesselInventory.Models;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        public RelayCommand<IClosable> LoginCommand { get; private set; }

        private readonly IWindowService _windowService;

        public LoginVM(IWindowService windowService)
        {
            _windowService = windowService;
            LoginCommand = new RelayCommand<IClosable>(LoginAction);
        }

        private User User
        {
            get; set;
        } = new User();

        [Required(ErrorMessage ="*")]
        public string username
        {
            get => User.username;
            set
            {
                User.username = value;
                OnPropertyChanged("username");
            }
        }

        [Required(ErrorMessage ="*")]
        public string password
        {
            get => User.password;
            set
            {
                User.password = value;
                OnPropertyChanged("password");
            }
        }

        private void LoginAction(IClosable window)
        {
            Auth.Instance.username = username;
            var container = (((App)Application.Current)).UnityContainer;
            _windowService.ShowWindow<MainWindow>(container.Resolve<HomeVM>());
            CloseWindow(window);
        }
    }
}
