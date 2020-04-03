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
using ToastNotifications.Messages;
using VesselInventory.Views;
using VesselInventory.Services;
using VesselInventory.Commons.HelperFunctions;

namespace VesselInventory.ViewModel
{
    public class RequestFormAddOrEditViewModel : ViewModelBase, IParentLoadable
    {
        private RequestFormShipBargeDTO _requestFormShipBargeDTO;
        private RF _rf = new RF();

        private readonly IRequestFormRepository _requestFormRepository;
        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IParentLoadable _parentLoadable;
        private readonly IWindowService _windowService;

        public RelayCommand<IClosable> Close { get; private set; }
        public RelayCommand Save { get; private set; }
        public RelayCommand AddOrEditItem { get; private set; }

        public RequestFormAddOrEditViewModel(IParentLoadable parentLoadable) : this(parentLoadable, 0) { }
        public RequestFormAddOrEditViewModel(IParentLoadable parentLoadable, int rf_id_params)
        {
            _parentLoadable = parentLoadable;
            _windowService = new WindowService();
            _requestFormRepository = new RequestFormRepository();
            _requestFormItemRepository = new RequestFormItemRepository();

            SetCommands();

            LoadDepartmentList();
            LoadAttributes(rf_id_params);
            if (rf_id_params != 0)
                LoadGrid();
        }

        private void SetCommands()
        {
            Close = new RelayCommand<IClosable>(CloseAction);
            Save = new RelayCommand(SaveAction, IsSaveCanExecute);
            AddOrEditItem = new RelayCommand(AddOrEditItemAction);
        }

        private ObservableCollection<string> _departmentList = new ObservableCollection<string>();
        public ObservableCollection<string> DepartmentList
        {
            get => _departmentList;
            set
            {
                _departmentList = value;
                OnPropertyChanged("DepartmentList");
            }
        }

        #region
        public int barge_id { get; set; }
        public string barge_name { get; set; }
        public string ship_code { get; set; }
        public string barge_code { get; set; }

        public int ship_id
        {
            get => _rf.ship_id;
            set => _rf.ship_id = value;
        }
        public int rf_id
        {
            get => _rf.rf_id;
            set => _rf.rf_id = value;
        }
        public string rf_number
        {
            get => _rf.rf_number;
            set
            {
                _rf.rf_number = value;
                OnPropertyChanged("rf_number");
            }
        }
        public string ship_name
        {
            get => _rf.ship_name;
            set
            {
                _rf.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        [Required(ErrorMessage ="Project Number cannot be empty.")]
        public string project_number {
            get => _rf.project_number;
            set
            {
                _rf.project_number = value;
                OnPropertyChanged("project_number");
            }
        }

        [Required]
        public string department_name
        {
            get => _rf.department_name;
            set
            {
                _rf.department_name = value;
                OnPropertyChanged("department_name");
            }
        }

        [Required]
        public DateTime target_delivery_date
        {
            get => _rf.target_delivery_date;
            set
            {
                _rf.target_delivery_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("target_delivery_date");
            }
        }
        
        [Column(TypeName = "text")]
        [Required]
        public string notes
        {
            get => _rf.notes;
            set
            {
                _rf.notes = value;
                OnPropertyChanged("notes");
            }
        }
        #endregion

        // <summary>
        // UI properties
        // </summary>
        #region
        public override string Title { get; set; }

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

        public ObservableCollection<RFItem> RfItemList { get; set; } = new ObservableCollection<RFItem>();
        #endregion

        private void LoadDepartmentList()
        {
            foreach(var department in DataHelper.GetLookupValues("DEPARTMENT") ) {
                _departmentList.Add(department.description);
            }
        }
        
        public void LoadGrid()
        {
            RfItemList.Clear();
            foreach (var _ in _requestFormItemRepository.GetRFItemList(rf_id))
                RfItemList.Add(_);
        }

        private void LoadAttributes(int rf_id_params)
        {
            if (rf_id_params != 0)
            {
                Title = "Edit Request Form";
                IsVisibleButtonUpdate = true;
                IsVisibleButtonSave = false;
                IsItemEnabled = true;
                IsVisibleBargeCheck = false;

                _rf = _requestFormRepository.FindById(rf_id_params);
            } else
            {
                Title = "Add Request Form";
                IsVisibleButtonSave = true;
                IsVisibleButtonUpdate = false;
                IsItemEnabled = false;
                IsVisibleBargeCheck = true;

                _rf.department_name = DepartmentList.First();
                _rf.target_delivery_date = DateTime.Now;

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
            {
                _rf.created_by = "Aditya";
                _rf = _requestFormRepository.SaveRequestForm(_rf);
            } else
            {
                _rf = _requestFormRepository.UpdateRequestForm(rf_id, _rf);
            }
            IsVisibleButtonSave = false;
            IsVisibleButtonUpdate = true;
            IsItemEnabled = true;
            Notifier toasMessage = ToastNotification.Instance.GetInstance();
            toasMessage.ShowSuccess("Data saved successfully.");

            _parentLoadable.LoadGrid();
        }
        private bool IsSaveCanExecute(object parameter)
        {
            if (project_number == string.Empty)
                return false;
            if (project_number is null)
                return false;
            if(project_number.Trim().Length < 0)
                return false;
            return true;
        }


        private void AddOrEditItemAction(object parameter)
        {
            if (parameter != null)
                _windowService.ShowWindow<RequestForm_ItemAddOrEditView>
                    (new RequestFormItemAddOrEditViewModel(this,rf_id,(int)parameter));
            else
                _windowService.ShowWindow<RequestForm_ItemAddOrEditView>
                    (new RequestFormItemAddOrEditViewModel(this,rf_id));
        }

        private void CloseAction(IClosable window)
        {
            if (window != null)
                window.Close();
        }
    }
}
