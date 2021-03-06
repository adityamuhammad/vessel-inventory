﻿using System;
using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;
using VesselInventory.Views;
using VesselInventory.Services;
using VesselInventory.Commons;
using VesselInventory.Commons.Enums;
using VesselInventory.Validations;
using Unity;
using System.Text;

namespace VesselInventory.ViewModel
{
    public class RequestFormAddOrEditVM : ViewModelBase, IDataGrid
    {
        public ObservableCollection<RequestFormItem> RequestFormItemCollection { get; set; } 
            = new ObservableCollection<RequestFormItem>();

        private readonly IRequestFormRepository _requestFormRepository;
        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IWindowService _windowService;
        private IDataGrid _parentLoadable;
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
        
        public void InitializeData(IDataGrid parentLoadable, int requestFormId = 0)
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

        public string DepartmentName
        {
            get
            {
                if (RequestFormDataView.DepartmentName is null)
                    RequestFormDataView.DepartmentName = Auth.Instance.DepartmentName;
                return RequestFormDataView.DepartmentName;
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
                if  (_IsVisibleButtonRelease == value) return;

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
                if  (_IsVisibleButtonUpdate == value) return;

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
                if  (_IsVisibleButtonSave == value) return;

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
                if  (_IsVisibleBargeCheck == value) return;

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
                if  (_IsItemEnabled == value) return;

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
                if (_IsCheckedBargeRequest == value) return;

                _IsCheckedBargeRequest = value;

                if (RequestFormShipBarge is null) return;

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
            get => _requestFormRepository.GetRrequestFormShipBarge();
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


        private IEnumerable<RequestFormItem> RequestFormItemList
        {
            get => _requestFormItemRepository.GetRequestFormItemList(RequestFormId);
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

            string attachmentFileName = (string)parameter;
            string fileLocation = GlobalNamespace.AttachmentPathLocation + attachmentFileName;

            if (string.IsNullOrWhiteSpace(attachmentFileName))
            {
                ResponseMessage.Info(GlobalNamespace.AttachmentNotUploaded);
                return;
            }

            if (!File.Exists(fileLocation)){
                ResponseMessage.Warning(GlobalNamespace.AttachmentMissing);
                return;
            }

            var previewPdf = UnityContainer.Resolve<PreviewPdf>();

            previewPdf.SetAttachment(fileLocation);

            previewPdf.ShowDialog();
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(RequestFormId))
            {
                CheckDocument();
                Save();
            }
            else
            {
                Update();
            }
        }
        private void Save()
        {
            RequestFormDataView.CreatedBy = Auth.Instance.PersonName;
            RequestFormDataView.CreatedDate = DateTime.Now;
            RequestFormDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
            RequestFormDataView.Status = Commons.Enums.Status.Draft.GetDescription();
            RequestFormDataView = _requestFormRepository.SaveTransaction(RequestFormDataView);
        }

        private void Update()
        {
            RequestFormDataView.LastModifiedBy = Auth.Instance.PersonName;
            RequestFormDataView.LastModifiedDate = DateTime.Now;
            RequestFormDataView = _requestFormRepository.Update(RequestFormId, RequestFormDataView);
        }

        private static void CheckDocument()
        {
            if (RequestValidator.IsAnyDraftDocument(Auth.Instance.DepartmentName, Auth.Instance.ShipId))
                throw new ValidationException(GlobalNamespace.DraftExists);
            if (RequestValidator.IsAnyDocumentCreatedInThreeDays(Auth.Instance.DepartmentName, Auth.Instance.ShipId))
                throw new ValidationException(GlobalNamespace.CanOnlyCreateDocumentPerThreeDays);
        }

        private void SaveAction(object parameter)
        {
            try
            {

                SaveOrUpdate();
                LoadParentDataGrid();
                SetUIEditProperties();

                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            }
            catch (ValidationException ex)
            {
                ResponseMessage.Error(string.Format("{0} {1}", GlobalNamespace.ErrorSave, ex.Message));
            } catch (Exception)
            {
                ResponseMessage.Error(string.Format("{0} {1}", 
                    GlobalNamespace.Error, 
                    GlobalNamespace.ErrorSave
                    ));
            }
        }

        private void LoadParentDataGrid()
        {
            _parentLoadable.LoadDataGrid();
        }

        private void DeleteItemAction(object parameter)
        {
            MessageBoxResult confirmDialog = DialogHelper.DialogConfirmation(
                GlobalNamespace.DeleteConfirmation,
                GlobalNamespace.DeleteConfirmationDescription);

            if (confirmDialog == MessageBoxResult.No) return;

            _requestFormItemRepository.Delete((int)parameter);

            ResponseMessage.Success(GlobalNamespace.SuccessDelete);

            LoadDataGrid();
        }

        private void ReleaseAction(IClosable window)
        {
            MessageBoxResult confirmDialog = DialogHelper.DialogConfirmation(
                GlobalNamespace.ReleaseConfirmation, 
                GlobalNamespace.ReleaseConfirmationDescription);

            if (confirmDialog == MessageBoxResult.No) return;

            _requestFormRepository.Release(RequestFormId);
            ResponseMessage.Success(GlobalNamespace.SuccesRelease);

            _parentLoadable.LoadDataGrid();

            CloseWindow(window);
        }

        private bool IsSaveCanExecute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(DepartmentName)) return false;

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
