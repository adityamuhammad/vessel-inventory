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
using System.IO;

namespace VesselInventory.ViewModel
{
    public class RequestFormAddOrEditVM : ViewModelBase, IParentLoadable
    {
        private readonly IRequestFormRepository _requestFormRepository;
        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IWindowService _windowService;
        private IParentLoadable _parentLoadable;
        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand PreviewPdfCommand { get; private set; }
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
        
        public void InitializeData(IParentLoadable parentLoadable, int requestFormId = 0)
        {
            _parentLoadable = parentLoadable;
            RequestFormId = requestFormId;
            LoadAttributes();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveAction, IsSaveCanExecute);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItemAction,IsAddOrEditItemCanExecute);
            DeleteItemCommand = new RelayCommand(DeleteItemAction,IsDeleteItemCanExecute);
            ReleaseCommand = new RelayCommand<IClosable>(ReleaseAction,IsReleaseCanExecute);
            PreviewPdfCommand = new RelayCommand(PreviewPdfAction);
        }

        /// <summary>
        /// Column or Field Attributes
        /// </summary>
        #region
        public string Status => RequestFormDataView.Status;
        public int BargeId { get; set; }
        public string BargeName { get; set; }
        public string ShipCode { get; set; }
        public string BargeCode { get; set; }

        public int ShipId
        {
            get => RequestFormDataView.ShipId;
            set => RequestFormDataView.ShipId = value;
        }
        public int RequestFormId
        {
            get => RequestFormDataView.RequestFormId;
            set => RequestFormDataView.RequestFormId = value;
        }

        public string RequestFormNumber
        {
            get => RequestFormDataView.RequestFormNumber;
            set
            {
                RequestFormDataView.RequestFormNumber = value;
                OnPropertyChanged("RequestFormNumber");
            }
        }
        public string ShipName
        {
            get => RequestFormDataView.ShipName;
            set
            {
                RequestFormDataView.ShipName = value;
                OnPropertyChanged("ShipName");
            }
        }

        public string ProjectNumber {
            get => RequestFormDataView.ProjectNumber;
            set
            {
                RequestFormDataView.ProjectNumber = value;
                OnPropertyChanged("ProjectNumber");
            }
        }

        [Required]
        [Display(Name ="Department Name")]
        public string DepartmentName
        {
            get
            {
                return RequestFormDataView.DepartmentName;
            }
            set
            {
                RequestFormDataView.DepartmentName = value;
                OnPropertyChanged("DepartmentName");
            }
        }

        [Required]
        public DateTime TargetDeliveryDate
        {
            get
            {
                if (RequestFormDataView.TargetDeliveryDate == default(DateTime) )
                    RequestFormDataView.TargetDeliveryDate = DateTime.Now.AddDays(4);
                return RequestFormDataView.TargetDeliveryDate;

            }
            set
            {
                RequestFormDataView.TargetDeliveryDate = DateTime.Parse(value.ToString());
                OnPropertyChanged("TargetDeliveryDate");
            }
        }
        
        [Column(TypeName = "text")]
        [Required]
        public string Notes
        {
            get => RequestFormDataView.Notes;
            set
            {
                RequestFormDataView.Notes = value;
                OnPropertyChanged("Notes");
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
                if (_IsCheckedBargeRequest == value)
                    return;
                _IsCheckedBargeRequest = value;

                if (RequestFormShipBarge is null)
                    return;

                RenameSequenceNumber();
                OnPropertyChanged("IsCheckedBargeRequest");
            }
        }

        private void RenameSequenceNumber()
        {
            if (_IsCheckedBargeRequest)
            {
                RequestFormNumber = string.Format("{0}-{1}-{2}", RequestFormShipBarge.RequestFormNumber, ShipCode, BargeCode);
                ShipName = RequestFormShipBarge.Bargename;
                ShipId = RequestFormShipBarge.BargeId;
            } else {
                RequestFormNumber = string.Format("{0}-{1}", RequestFormShipBarge.RequestFormNumber, ShipCode);
                ShipName = RequestFormShipBarge.ShipName;
                ShipId = RequestFormShipBarge.ShipId;
            }
        }
        #endregion

        /// <summary>
        /// UI Collections, Entity and Custom Column Attributes
        /// </summary>
        #region
        public bool IsReleased => (Status == Commons.Enums.Status.Release.GetDescription());
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

        private RequestForm RequestFormDataView { get; set; } = new RequestForm();

        public IList<string> DepartmentCollection
        {
            get
            {
                IList<string> departmens = new List<string>();
                foreach (var _ in CommonDataHelper.GetLookupValues("DEPARTMENT"))
                    departmens.Add(_.Descriptions);
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
                    .GetRequestFormItemList(RequestFormId);
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

        private void SetValueAttributes()
        {
            ShipCode = RequestFormShipBarge.ShipCode;
            BargeCode = RequestFormShipBarge.BargeCode;
            ShipId = RequestFormShipBarge.ShipId;
            BargeId = RequestFormShipBarge.BargeId;
            ShipName = RequestFormShipBarge.ShipName;
            BargeName = RequestFormShipBarge.Bargename;
            RequestFormNumber = string.Format("{0}-{1}", RequestFormShipBarge.RequestFormNumber, ShipCode);
        }

        private void LoadAttributes()
        {
            if (!RecordHelper.IsNewRecord(RequestFormId))
            {
                SetUIEditProperties();
                RequestFormDataView = _requestFormRepository.GetById(RequestFormId);
                LoadDataGrid();
            }
            else
            {
                SetUIAddProperties();
                SetValueAttributes();
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
            var requestFormItemAddOrEditVM = UnityContainer.Resolve<RequestFormItemAddOrEditVM>();
            if (parameter is null)
                requestFormItemAddOrEditVM.InitializeData(this, RequestFormId);
            else
                requestFormItemAddOrEditVM.InitializeData(this, RequestFormId, (int)parameter);
            _windowService.ShowDialogWindow<RequestForm_ItemAddOrEditView>(requestFormItemAddOrEditVM);
        }

        private void PreviewPdfAction(object parameter)
        {

            string attachmentLocation = (string)parameter;
            if (string.IsNullOrWhiteSpace(attachmentLocation))
            {
                ResponseMessage.Info(GlobalNamespace.AttachmentNotUploaded);
                return;
            }
            if (!File.Exists(attachmentLocation)){
                ResponseMessage.Warning(GlobalNamespace.AttachmentMissing);
                return;
            }
            var previewPdf = UnityContainer.Resolve<PreviewPdf>();
            previewPdf.SetAttachment(attachmentLocation);
            previewPdf.ShowDialog();
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(RequestFormId))
            {
                RequestFormDataView = _requestFormRepository
                    .SaveTransaction(RequestFormDataView);

            } else
            {
                RequestFormDataView.LastModifiedBy = Auth.Instance.personalname;
                RequestFormDataView.LastModifiedDate = DateTime.Now;
                RequestFormDataView = _requestFormRepository
                    .Update(RequestFormId, RequestFormDataView);

            }
        }
        private void SaveAction(object parameter)
        {
            try
            {
                SaveOrUpdate();
                _parentLoadable.LoadDataGrid();
                SetUIEditProperties();
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            } catch (Exception ex)
            {
                ResponseMessage.Error(string.Format("{0} {1} {2}", 
                    GlobalNamespace.Error, 
                    GlobalNamespace.ErrorSave,
                    ex.Message));
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
            _requestFormRepository.Release(RequestFormId);
            ResponseMessage.Success(GlobalNamespace.SuccesRelease);
            _parentLoadable.LoadDataGrid();
            CloseWindow(window);
        }

        private bool IsSaveCanExecute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(DepartmentName))
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
