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
using System.Collections.Generic;

namespace VesselInventory.ViewModel
{
    public class RequestFormAddOrEditViewModel : ViewModelBase, IParentLoadable
    {
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
        public RequestFormAddOrEditViewModel(IParentLoadable parentLoadable, int rf_id)
        {
            _parentLoadable = parentLoadable;
            _windowService = new WindowService();
            _requestFormRepository = new RequestFormRepository();
            _requestFormItemRepository = new RequestFormItemRepository();
            this.rf_id = rf_id;

            InitializeCommands();
            LoadAttributes();
            if (!IsNewRecord)
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

        /// <summary>
        /// Attributes
        /// </summary>
        #region
        public string status => RequestFormEntity.status;
        public int barge_id { get; set; }
        public string barge_name { get; set; }
        public string ship_code { get; set; }
        public string barge_code { get; set; }

        public int ship_id
        {
            get => RequestFormEntity.ship_id;
            set => RequestFormEntity.ship_id = value;
        }
        public int rf_id
        {
            get => RequestFormEntity.rf_id;
            set => RequestFormEntity.rf_id = value;
        }

        public string rf_number
        {
            get => RequestFormEntity.rf_number;
            set
            {
                RequestFormEntity.rf_number = value;
                OnPropertyChanged("rf_number");
            }
        }
        public string ship_name
        {
            get => RequestFormEntity.ship_name;
            set
            {
                RequestFormEntity.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        [Required(ErrorMessage ="Project Number cannot be empty.")]
        public string project_number {
            get => RequestFormEntity.project_number;
            set
            {
                RequestFormEntity.project_number = value;
                OnPropertyChanged("project_number");
            }
        }

        public bool IsReleased
        {
            get
            {
                return (status == Status.RELEASE.GetDescription());
            }
        }

        [Required]
        public string department_name
        {
            get
            {
                if (RequestFormEntity.department_name is null)
                    RequestFormEntity.department_name = DepartmentCollection.First();
                return RequestFormEntity.department_name;
            }
            set
            {
                RequestFormEntity.department_name = value;
                OnPropertyChanged("department_name");
            }
        }

        [Required]
        public DateTime target_delivery_date
        {
            get
            {
                if (RequestFormEntity.target_delivery_date == default(DateTime) )
                    RequestFormEntity.target_delivery_date = DateTime.Now;
                return RequestFormEntity.target_delivery_date;

            }
            set
            {
                RequestFormEntity.target_delivery_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("target_delivery_date");
            }
        }
        
        [Column(TypeName = "text")]
        [Required]
        public string notes
        {
            get => RequestFormEntity.notes;
            set
            {
                RequestFormEntity.notes = value;
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

                if (RequestFormShipBarge is null)
                    return;

                if (_IsCheckedBargeRequest)
                {
                    rf_number = RequestFormShipBarge.rf_number + '-' + ship_code + '-' + barge_code;
                    ship_name = RequestFormShipBarge.barge_name;
                    ship_id = RequestFormShipBarge.barge_id;
                }
                else
                {
                    rf_number = RequestFormShipBarge.rf_number + '-' + ship_code;
                    ship_name = RequestFormShipBarge.ship_name;
                    ship_id = RequestFormShipBarge.ship_id;
                }
                OnPropertyChanged("IsCheckedBargeRequest");
            }
        }

        #endregion

        /// <summary>
        /// UI Collections and Entity
        /// </summary>
        #region
        private bool IsNewRecord
        {
            get
            {
                return (rf_id == 0);
            }
        }
        private RequestFormShipBargeDTO RequestFormShipBarge
        {
            get
            {
                return _requestFormRepository.GetRrequestFormShipBarge();
            }
        }

        private RequestForm RequestFormEntity
        {
            get;
            set;
        } = new RequestForm();

        public IList<string> DepartmentCollection
        {
            get
            {
                List<string> departmens = new List<string>();
                foreach (var _ in DataHelper.GetLookupValues("DEPARTMENT"))
                    departmens.Add(_.description);
                return departmens;
            }
        }

        public ObservableCollection<RequestFormItem> RequestFormItemCollection { get; set; } 
            = new ObservableCollection<RequestFormItem>();
        private IEnumerable<RequestFormItem> RequestFormItemList
        {
            get
            {
                return _requestFormItemRepository.GetRFItemList(rf_id);
            }
        }
        #endregion

        /// <summary>
        /// Load UI methods
        /// </summary>
        #region
        public void LoadDataGrid()
        {
            RequestFormItemCollection.Clear();
            foreach (var _ in RequestFormItemList)
                RequestFormItemCollection.Add(_);
            TotalItem = RequestFormItemCollection.Count;
        }

        private void SetUIEditAttributes()
        {
            Title = "Edit Request Form";
            IsVisibleButtonUpdate = true;
            IsVisibleButtonRelease = true;
            IsVisibleButtonSave = false;
            IsItemEnabled = true;
            IsVisibleBargeCheck = false;
        }

        private void SetUIAddAttributes()
        {
            Title = "Add Request Form";
            IsVisibleButtonSave = true;
            IsVisibleButtonUpdate = false;
            IsVisibleButtonRelease = false;
            IsItemEnabled = false;
            IsVisibleBargeCheck = true;
        }

        private void SetFormAttributes(int rf_id_params)
        {
            ship_code = RequestFormShipBarge.ship_code;
            barge_code = RequestFormShipBarge.barge_code;
            ship_id = RequestFormShipBarge.ship_id;
            barge_id = RequestFormShipBarge.barge_id;
            ship_name = RequestFormShipBarge.ship_name;
            barge_name = RequestFormShipBarge.barge_name;
            rf_number = RequestFormShipBarge.rf_number + '-' + ship_code;
        }

        private void LoadAttributes()
        {
            if (!IsNewRecord)
            {
                SetUIEditAttributes();
                RequestFormEntity = _requestFormRepository.GetById(rf_id);
            } else
            {
                SetUIAddAttributes();
                SetFormAttributes(rf_id);
            }
        }
        #endregion

        /// <summary>
        /// Button actions and behavior
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void SaveAction(object parameter)
        {
            if (rf_id == 0)
                RequestFormEntity = _requestFormRepository.SaveRequestForm(RequestFormEntity);
            else
                RequestFormEntity = _requestFormRepository.Update(rf_id, RequestFormEntity);

            SetUIEditAttributes();
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
                    (new RequestFormItemAddOrEditViewModel(
                        this,
                        new UploadService(),
                        new OpenPdfFileDialog(),
                        new RequestFormItemRepository(),
                        rf_id,
                        (int)parameter));
            else
                _windowService.ShowWindow<RequestForm_ItemAddOrEditView>
                    (new RequestFormItemAddOrEditViewModel
                        (
                            this,
                            new UploadService(),
                            new OpenPdfFileDialog(),
                            new RequestFormItemRepository(),
                            rf_id
                        )
                      );
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
            MessageBoxResult confirmDialog = UIHelper.DialogConfirmation("Delete Confirmation","Are you sure?" );
            if (confirmDialog == MessageBoxResult.No)
                return;
            _requestFormItemRepository.Delete((int)parameter);
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
            CloseWindow(window);
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
        #endregion
    }
}
