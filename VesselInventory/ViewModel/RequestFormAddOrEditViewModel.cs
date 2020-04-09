using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ToastNotifications;
using VesselInventory.DTO;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;
using VesselInventory.Views;
using VesselInventory.Services;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Commons;
using System.Windows;
using VesselInventory.Commons.Enums;
using ToastNotifications.Messages;

namespace VesselInventory.ViewModel
{
    public class RequestFormAddOrEditViewModel : ViewModelBase, IParentLoadable
    {
        private RequestFormShipBargeDTO _requestFormShipBargeDTO;
        private RequestForm _requestForm = new RequestForm();
        private Notifier _toasMessage = ToastNotification.Instance.GetInstance();

        private readonly IRequestFormRepository _requestFormRepository;
        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IParentLoadable _parentLoadable;
        private readonly IWindowService _windowService;

        public RelayCommand<IClosable> CloseCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        public RelayCommand<IClosable> ReleaseCommand { get; private set; }

        public RequestFormAddOrEditViewModel(IParentLoadable parentLoadable) : this(parentLoadable, 0) { }
        public RequestFormAddOrEditViewModel(IParentLoadable parentLoadable, int rf_id_params)
        {
            _parentLoadable = parentLoadable;
            _windowService = new WindowService();
            _requestFormRepository = new RequestFormRepository();
            _requestFormItemRepository = new RequestFormItemRepository();

            InitializeCommands();

            LoadDepartmentList();
            LoadAttributes(rf_id_params);

            if (rf_id_params != 0)
                LoadDataGrid();
        }

        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand<IClosable>(CloseWindow);
            SaveCommand = new RelayCommand(SaveAction, IsSaveCanExecute);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItem,IsCanExecuteAddOrEditItem);
            DeleteItemCommand = new RelayCommand(DeleteItem,IsCanExecuteDeleteItem);
            ReleaseCommand = new RelayCommand<IClosable>(Release,IsReleaseCanExecute);
        }

        #region
        public string status => _requestForm.status;
        public int barge_id { get; set; }
        public string barge_name { get; set; }
        public string ship_code { get; set; }
        public string barge_code { get; set; }

        public int ship_id
        {
            get => _requestForm.ship_id;
            set => _requestForm.ship_id = value;
        }
        public int rf_id
        {
            get => _requestForm.rf_id;
            set => _requestForm.rf_id = value;
        }

        public string rf_number
        {
            get => _requestForm.rf_number;
            set
            {
                _requestForm.rf_number = value;
                OnPropertyChanged("rf_number");
            }
        }
        public string ship_name
        {
            get => _requestForm.ship_name;
            set
            {
                _requestForm.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        [Required(ErrorMessage ="Project Number cannot be empty.")]
        public string project_number {
            get => _requestForm.project_number;
            set
            {
                _requestForm.project_number = value;
                OnPropertyChanged("project_number");
            }
        }

        public bool IsReleased
        {
            get
            {
                if (status == Status.RELEASE.GetDescription())
                    return true;
                return false;
            }
        }

        [Required]
        public string department_name
        {
            get => _requestForm.department_name;
            set
            {
                _requestForm.department_name = value;
                OnPropertyChanged("department_name");
            }
        }

        [Required]
        public DateTime target_delivery_date
        {
            get => _requestForm.target_delivery_date;
            set
            {
                _requestForm.target_delivery_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("target_delivery_date");
            }
        }
        
        [Column(TypeName = "text")]
        [Required]
        public string notes
        {
            get => _requestForm.notes;
            set
            {
                _requestForm.notes = value;
                OnPropertyChanged("notes");
            }
        }
        #endregion

        // <summary>
        // UI properties
        // </summary>
        #region
        public override string Title { get; set; }

        private bool _IsVisibleButtonRelease;
        public bool IsVisibleButtonRelease
        {
            get => _IsVisibleButtonRelease;
            set
            {
                if  (_IsVisibleButtonRelease == value)
                    return;
                _IsVisibleButtonRelease = value;
                OnPropertyChanged("IsVisibleButtonRelease");
            }
        }
        private bool _IsVisibleButtonUpdate;
        public bool IsVisibleButtonUpdate
        {
            get => _IsVisibleButtonUpdate;
            set
            {
                if  (_IsVisibleButtonUpdate == value)
                    return;
                _IsVisibleButtonUpdate = value;
                OnPropertyChanged("IsVisibleButtonUpdate");
            }
        }

        private bool _IsVisibleButtonSave;
        public bool IsVisibleButtonSave
        {
            get => _IsVisibleButtonSave;
            set
            {
                if  (_IsVisibleButtonSave == value)
                    return;
                _IsVisibleButtonSave = value;
                OnPropertyChanged("IsVisibleButtonSave");
            }
        }

        private bool _IsVisibleBargeCheck;
        public bool IsVisibleBargeCheck
        {
            get => _IsVisibleBargeCheck;
            set
            {
                if  (_IsVisibleBargeCheck == value)
                    return;
                _IsVisibleBargeCheck = value;
                OnPropertyChanged("IsVisiblBargeCheck");
            }
        }

        private bool _IsItemEnabled;
        public bool IsItemEnabled
        {
            get => _IsItemEnabled;
            set
            {
                if  (_IsItemEnabled == value)
                    return;
                _IsItemEnabled = value;
                OnPropertyChanged("IsItemEnabled");
            }
        }

        private bool _IsCheckedBargeRequest = false;
        public bool IsCheckedBargeRequest
        {
            get => _IsCheckedBargeRequest;
            set
            {
                if  (_IsCheckedBargeRequest == value)
                    return;
                _IsCheckedBargeRequest = value;

                if (_requestFormShipBargeDTO is null)
                    return;

                if (_IsCheckedBargeRequest)
                {
                    rf_number = _requestFormShipBargeDTO.rf_number + '-' + ship_code + '-' + barge_code;
                    ship_name = _requestFormShipBargeDTO.barge_name;
                    ship_id = _requestFormShipBargeDTO.barge_id;
                }
                else
                {
                    rf_number = _requestFormShipBargeDTO.rf_number + '-' + ship_code;
                    ship_name = _requestFormShipBargeDTO.ship_name;
                    ship_id = _requestFormShipBargeDTO.ship_id;
                }
                OnPropertyChanged("IsCheckedBargeRequest");
            }
        }

        public ObservableCollection<string> DepartmentCollection { get; set; } 
            = new ObservableCollection<string>();
        public ObservableCollection<RequestFormItem> RequestFormItemCollection { get; set; } 
            = new ObservableCollection<RequestFormItem>();
        #endregion

        private void LoadDepartmentList()
        {
            foreach(var department in DataHelper.GetLookupValues("DEPARTMENT"))
                DepartmentCollection.Add(department.description);
        }
        
        public void LoadDataGrid()
        {
            RequestFormItemCollection.Clear();
            foreach (var _ in _requestFormItemRepository.GetRFItemList(rf_id))
                RequestFormItemCollection.Add(_);
            TotalItem = RequestFormItemCollection.Count;
        }

        private void LoadAttributes(int rf_id_params)
        {
            if (rf_id_params != 0)
            {
                Title = "Edit Request Form";
                IsVisibleButtonUpdate = true;
                IsVisibleButtonRelease = true;
                IsVisibleButtonSave = false;
                IsItemEnabled = true;
                IsVisibleBargeCheck = false;

                _requestForm = _requestFormRepository.GetById(rf_id_params);
            } else
            {
                Title = "Add Request Form";
                IsVisibleButtonSave = true;
                IsVisibleButtonUpdate = false;
                IsVisibleButtonRelease = false;
                IsItemEnabled = false;
                IsVisibleBargeCheck = true;

                _requestForm.department_name = DepartmentCollection.First();
                _requestForm.target_delivery_date = DateTime.Now;

                _requestFormShipBargeDTO = _requestFormRepository.GetRrequestFormShipBarge();
                rf_id = rf_id_params;
                ship_code = _requestFormShipBargeDTO.ship_code;
                barge_code = _requestFormShipBargeDTO.barge_code;
                ship_id = _requestFormShipBargeDTO.ship_id;
                barge_id = _requestFormShipBargeDTO.barge_id;
                ship_name = _requestFormShipBargeDTO.ship_name;
                barge_name = _requestFormShipBargeDTO.barge_name;
                rf_number = _requestFormShipBargeDTO.rf_number + '-' + ship_code;
            }
        }

        private void SaveAction(object parameter)
        {
            if (rf_id == 0)
                _requestForm = _requestFormRepository.SaveRequestForm(_requestForm);
            else
                _requestForm = _requestFormRepository.Update(rf_id, _requestForm);

            IsVisibleButtonSave = false;
            IsVisibleButtonUpdate = true;
            IsVisibleButtonRelease = true;
            IsItemEnabled = true;
            _toasMessage.ShowSuccess("Data saved successfully.");

            _parentLoadable.LoadDataGrid();
        }
        private bool IsSaveCanExecute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(project_number))
                return false;

            return IsReleasedCanExecute();
        }


        private void AddOrEditItem(object parameter)
        {
            if (parameter != null)
                _windowService.ShowWindow<RequestForm_ItemAddOrEditView>
                    (new RequestFormItemAddOrEditViewModel(this,rf_id,(int)parameter));
            else
                _windowService.ShowWindow<RequestForm_ItemAddOrEditView>
                    (new RequestFormItemAddOrEditViewModel(this,rf_id));
        }

        private void CloseWindow(IClosable window)
        {
            if (window != null)
                window.Close();
        }

        private int _totalItem = 0;
        public int TotalItem
        {
            get => _totalItem;
            set
            {
                _totalItem = value;
                OnPropertyChanged("TotalItem");
            }
        }

        private void DeleteItem(object parameter)
        {
            MessageBoxResult confirmDialog = UIHelper.DialogConfirmation("Are you sure?", "Delete Confirmation");
            if (confirmDialog == MessageBoxResult.No)
                return;
            _requestFormItemRepository.Delete(int.Parse(parameter.ToString()));
            _toasMessage.ShowSuccess("Data deleted successfully.");
            LoadDataGrid();
        }

        private void Release(IClosable window)
        {
            MessageBoxResult confirmDialog = UIHelper.DialogConfirmation("Release Confirmation", "Are you sure?");
            if (confirmDialog == MessageBoxResult.No)
                return;

            _requestFormRepository.Release(rf_id);
            _toasMessage.ShowSuccess("Release successfully to process.");
            _parentLoadable.LoadDataGrid();

            if (window != null)
                window.Close();
        }

        private bool IsReleaseCanExecute(object parameter)
        {
            return IsReleasedCanExecute();
        }

        private bool IsCanExecuteAddOrEditItem(object parameter)
        {
            return IsReleasedCanExecute();
        }
        private bool IsCanExecuteDeleteItem(object parameter)
        {
            return IsReleasedCanExecute();
        }

        private bool IsReleasedCanExecute()
        {
            return !IsReleased;
        }
    }
}
