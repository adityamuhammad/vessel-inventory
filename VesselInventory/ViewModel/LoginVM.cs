using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Unity;
using VesselInventory.Commons;
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
        private readonly IAuthenticationService _authenticationService;

        public LoginVM(IWindowService windowService,IAuthenticationService authenticationService)
        {
            _windowService = windowService;
            _authenticationService = authenticationService;
            LoginCommand = new RelayCommand<IClosable>(LoginAction);
        }

        private string _personName;
        [Required(ErrorMessage ="*")]
        public string PersonName
        {
            get => _personName;
            set
            {
                _personName = value;
                OnPropertyChanged("personalname");
            }
        }

        private string _username;
        [Required(ErrorMessage ="*")]
        public string Username
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
        public string Password
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
            try
            {
                if (string.IsNullOrWhiteSpace(PersonName))
                    throw new Exception();
                var authentication = _authenticationService.Authenticate(Username, Password);

                if (authentication != null)
                {
                    Auth.Instance.UserId = authentication.UserId;
                    Auth.Instance.Username = authentication.Username;
                    Auth.Instance.DepartmentName = authentication.DepartmentName;
                    Auth.Instance.ShipId = authentication.ShipId;
                    Auth.Instance.PersonName = PersonName.ToUpper();
                    var container = (((App)Application.Current)).UnityContainer;
                    _windowService.ShowWindow<MainWindow>(container.Resolve<HomeVM>());
                    CloseWindow(window);
                    ResponseMessage.Success(GlobalNamespace.SuccessLogin);
                } else
                {
                    ResponseMessage.Error(GlobalNamespace.FailedLogin);
                }
            }
            catch
            {
                ResponseMessage.Error(string.Format("{0} {1}", GlobalNamespace.Error, GlobalNamespace.WrongInput));
            }
        }
    }
}
