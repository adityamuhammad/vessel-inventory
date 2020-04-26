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
        public override string Title => "Login";
        public RelayCommand<IClosable> LoginCommand { get; private set; }

        private readonly IWindowService _windowService;

        public LoginVM(IWindowService windowService)
        {
            _windowService = windowService;
            LoginCommand = new RelayCommand<IClosable>(LoginAction);
        }

        private string _personalname;
        [Required(ErrorMessage ="*")]
        public string personalname
        {
            get => _personalname;
            set
            {
                _personalname = value;
                OnPropertyChanged("personalname");
            }
        }

        private string _username;
        [Required(ErrorMessage ="*")]
        public string username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged("username");
            }
        }

        private string _password;
        [Required(ErrorMessage ="*")]
        public string password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("password");
            }
        }

        private void LoginAction(IClosable window)
        {
            Auth.Instance.username = username;
            Auth.Instance.personalname = personalname.ToUpper();
            var container = (((App)Application.Current)).UnityContainer;
            _windowService.ShowWindow<MainWindow>(container.Resolve<HomeVM>());
            CloseWindow(window);
        }
    }
}
