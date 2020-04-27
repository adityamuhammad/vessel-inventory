using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;
using VesselInventory.Views;
using VesselInventory.Services;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Commons;
using System.Windows;
using VesselInventory.Commons.Enums;
using System.Collections.Generic;
using Unity;

namespace VesselInventory.ViewModel
{
    public class RequestFormAddOrEditVM : ViewModelBase, IParentLoadable
    {
        private readonly IRequestFormRepository _requestFormRepository;
        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IWindowService _windowService;
        private IParentLoadable _parentLoadable;

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        public RelayCommand<IClosable> ReleaseCommand { get; private set; }

        public RequestFormAddOrEditVM(IWindowService windowService, 
            IRequestFormRepository requestFormRepository, 
            IRequestFormItemRepository requestFormItemRepository)
        {
            _windowService = windowService;
            _requestFormRepository = requestFormRepository;
            _requestFormItemRepository = requestFormItemRepository;
            InitializeCommands();

        }
        
        public void InitializeData( IParentLoadable parentLoadable, 
            int requestFormId = 0)
        {
            _parentLoadable = parentLoadable;
            this.rf_id = requestFormId;
            LoadAttributes();
            LoadDataGrid();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveAction, IsSaveCanExecute);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItemAction,IsAddOrEditItemCanExecute);
            DeleteItemCommand = new RelayCommand(DeleteItemAction,IsDeleteItemCanExecute);
            ReleaseCommand = new RelayCommand<IClosable>(ReleaseAction,IsReleaseCanExecute);
        }

        /// <summary>
        /// Column or Field Attributes
        /// </summary>
        #region
        public string status => RequestFormDataView.status;
        public int barge_id { get; set; }
        public string barge_name { get; set; }
        public string ship_code { get; set; }
        public string barge_code { get; set; }

        public int ship_id
        {
            get => RequestFormDataView.ship_id;
            set => RequestFormDataView.ship_id = value;
        }
        public int rf_id
        {
            get => RequestFormDataView.rf_id;
            set => RequestFormDataView.rf_id = value;
        }

        public string rf_number
        {
            get => RequestFormDataView.rf_number;
            set
            {
                RequestFormDataView.rf_number = value;
                OnPropertyChanged("rf_number");
            }
        }
        public string ship_name
        {
            get => RequestFormDataView.ship_name;
            set
            {
                RequestFormDataView.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        public string project_number {
            get => RequestFormDataView.project_number;
            set
            {
                RequestFormDataView.project_number = value;
                OnPropertyChanged("project_number");
            }
        }

        [Required]
        [Display(Name ="Department Name")]
        public string department_name
        {
            get
            {
                return RequestFormDataView.department_name;
            }
            set
            {
                RequestFormDataView.department_name = value;
                OnPropertyChanged("department_name");
            }
        }

        [Required]
        public DateTime target_delivery_date
        {
            get
            {
                if (RequestFormDataView.target_delivery_date == default(DateTime) )
                    RequestFormDataView.target_delivery_date = DateTime.Now.AddDays(4);
                return RequestFormDataView.target_delivery_date;

            }
            set
            {
                RequestFormDataView.target_delivery_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("target_delivery_date");
            }
        }
        
        [Column(TypeName = "text")]
        [Required]
        public string notes
        {
            get => RequestFormDataView.notes;
            set
            {
                RequestFormDataView.notes = value;
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
        /// UI Collections, Entity and Custom Column Attributes
        /// </summary>
        #region
        public bool IsReleased => (status == Status.Release.GetDescription());
        private RequestFormShipBargeDto RequestFormShipBarge
        {
            get
            {
                return _requestFormRepository.GetRrequestFormShipBarge();
            }
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

        private RequestForm RequestFormDataView
        {
            get;
            set;
        } = new RequestForm();

        public IList<string> DepartmentCollection
        {
            get
            {
                IList<string> departmens = new List<string>();
                foreach (var _ in CommonDataHelper.GetLookupValues("DEPARTMENT"))
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
                return _requestFormItemRepository
                    .GetRequestFormItemList(rf_id);
            }
        }
        #endregion

        /// <summary>
        /// Load UI methods
        /// </summary>
        #region
        public void LoadDataGrid()
        {
            if (RecordHelper.IsNewRecord(rf_id)) return;
            RequestFormItemCollection.Clear();
            foreach (var _ in RequestFormItemList)
                RequestFormItemCollection.Add(_);
            TotalItem = RequestFormItemCollection.Count;
        }

        private void SetUIEditProperties()
        {
            Title = "Edit Request Form";
            IsVisibleButtonUpdate = true;
            IsVisibleButtonRelease = true;
            IsVisibleButtonSave = false;
            IsItemEnabled = true;
            IsVisibleBargeCheck = false;
        }

        private void SetUIAddProperties()
        {
            Title = "Add Request Form";
            IsVisibleButtonSave = true;
            IsVisibleButtonUpdate = false;
            IsVisibleButtonRelease = false;
            IsItemEnabled = false;
            IsVisibleBargeCheck = true;
        }

        private void SetAttributes(int rf_id_params)
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
            if (!RecordHelper.IsNewRecord(rf_id))
            {
                SetUIEditProperties();
                RequestFormDataView = _requestFormRepository.GetById(rf_id);
            } else
            {
                SetUIAddProperties();
                SetAttributes(rf_id);
            }
        }
        #endregion

        /// <summary>
        /// Button actions and behavior
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void AddOrEditItemAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            var requestFormItemAddOrEditVM = container.Resolve<RequestFormItemAddOrEditVM>();
            if (parameter is null)
                requestFormItemAddOrEditVM.InitializeData(this, rf_id);
            else
                requestFormItemAddOrEditVM.InitializeData(this, rf_id, (int)parameter);
            _windowService.ShowDialogWindow<RequestForm_ItemAddOrEditView>(requestFormItemAddOrEditVM);
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(rf_id))
            {
                RequestFormDataView = _requestFormRepository
                    .SaveTransaction(RequestFormDataView);

            } else
            {
                RequestFormDataView.last_modified_by = Auth.Instance.personalname;
                RequestFormDataView.last_modified_date = DateTime.Now;
                RequestFormDataView = _requestFormRepository
                    .Update(rf_id, RequestFormDataView);

            }
        }
        private void SaveAction(object parameter)
        {
            try
            {
                SaveOrUpdate();
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
                _parentLoadable.LoadDataGrid();
                SetUIEditProperties();
            } catch (Exception ex)
            {
                ResponseMessage.Error(GlobalNamespace.Error + ' '
                    + GlobalNamespace.ErrorSave + ' ' + ex.InnerException.Message);
            }
        }
        private void DeleteItemAction(object parameter)
        {
            MessageBoxResult confirmDialog = UIHelper.DialogConfirmation(
                GlobalNamespace.DeleteConfirmation,
                GlobalNamespace.DeleteConfirmationDescription);
            if (confirmDialog == MessageBoxResult.No)
                return;
            _requestFormItemRepository.Delete((int)parameter);
            ResponseMessage.Success(GlobalNamespace.SuccessDelete);
            LoadDataGrid();
        }

        private void ReleaseAction(IClosable window)
        {
            MessageBoxResult confirmDialog = UIHelper.DialogConfirmation(
                GlobalNamespace.ReleaseConfirmation, 
                GlobalNamespace.ReleaseConfirmationDescription);
            if (confirmDialog == MessageBoxResult.No)
                return;
            _requestFormRepository.Release(rf_id);
            ResponseMessage.Success(GlobalNamespace.SuccesRelease);
            _parentLoadable.LoadDataGrid();
            CloseWindow(window);
        }

        private bool IsSaveCanExecute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(department_name))
                return false;
            return IsReleasedCanExecute();
        }

        private bool IsReleaseCanExecute(object parameter)
        {
            return IsReleasedCanExecute();
        }

        private bool IsAddOrEditItemCanExecute(object parameter)
        {
            return IsReleasedCanExecute();
        }
        private bool IsDeleteItemCanExecute(object parameter)
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
